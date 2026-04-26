# 1. 关键业务逻辑说明

## 1.1. 短链接生成策略

### 核心逻辑
项目没有使用 Hash 算法（如 MD5/SHA）来生成短码，而是采用了 **基于数据库自增 ID 的 Base62 可逆混淆算法**。

*   **代码位置**: `ShortUrlService.WebApi/Shorteners/Base62Shortener.cs`
*   **流程**:
    1.  **查重**: 请求进来，先查 DB/Redis 是否已有该原始 URL。如有，直接返回旧短码。
    2.  **占位**: 如果没有，先插入数据库 `ShortUrl` 表，获取一个唯一的自增 `Id` (long)。
    3.  **生成**: 调用 `shortener.Short(id)` 将 `Id` 转换为短码 `ShortKey`。
    4.  **更新**: 将生成的 `ShortKey` 更新回数据库记录。

### 为什么这样做？
*   **唯一性保证**: Hash 算法存在碰撞概率，解决碰撞（如加盐重试）在高并发下会带来不可控的延迟。基于 ID 生成则天然保证唯一，因为 ID 是唯一的。
*   **无状态解析**: 解析短码时，`shortener.Restore(key)` 可以直接通过算法逆向还原出 ID，**不需要查询数据库索引**来匹配 ShortKey（虽然在 Redis 做了缓存，但这种算法特性是兜底保障）。

### 细节与坑
*   **混淆机制**: 直接转 Base62 会导致 ID `1` 生成短码 `1`，ID `2` 生成 `2`，容易被用户猜出规律并遍历数据。
    *   **解决方案**: 在 `Short` 方法里做了“位运算级”的混淆：`PadLeft` (补零) -> `Reverse` (倒序) -> `Encode` (Base62)。
    *   **密钥**: `ShortUrlConfig.Secrect` 并不是加密密码，而是 Base62 的**字符集顺序**（乱序后的 62 个字符）。**千万不要修改这个配置**，否则所有历史短链都会解析错误！

## 1.2. 访问跳转与并发处理

### 核心逻辑
短链接的访问量通常是生成的数千倍，因此 `AccessAsync` 接口必须极致轻量。

*   **代码位置**: `ShortUrlService.WebApi/Services/ShortUrlService.cs` -> `AccessAsync`

### 关键步骤
1.  **还原 ID**: `shortener.Restore(key)` 算出 ID。
2.  **查询缓存**: 优先查 Redis `ShortUrl:{id}`。如果穿透，查 DB 并回填 Redis（缓存时间 = 剩余有效期）。
3.  **规则校验**: 
    *   **有效期**: 比较 `ExpiredTime`。
    *   **访问上限**: 这是并发热点。**没有**每次都查 DB 的 `AccessLogs` 表 count。
    *   **计数器**: 使用 Redis 的原子操作 `StringIncrementAsync` 维护 `ShortUrl:{id}:AccessCount`。只有当 Redis 里的计数超过 `AccessLimit` 时，才拦截访问。

### 维护注意
*   **Redis 强依赖**: 虽然有 DB 兜底，但为了性能，在判断访问上限时严重依赖 Redis。如果 Redis 挂了，`RedisRetryProxy` 会尝试重试，但最终可能会降级（此时要注意 DB 压力）。

## 1.3. 访问日志

### 核心逻辑
为了实现“点击即跳转”，不能在用户请求的主线程中同步写入 MySQL 日志（IO 也是瓶颈）。为此实现了一个**内存队列 + 后台批量入库**的机制。

*   **代码位置**: 
    *   队列: `ShortUrlService.WebApi/Services/AccessLogQueue.cs`
    *   消费者: `ShortUrlService.WebApi/Services/AccessLogHostedService.cs`

### 架构设计
1.  **生产者**: `AccessAsync` 接口中，构建 `AccessLog` 对象，调用 `_queue.EnqueueAsync` 扔进 `System.Threading.Channels` (无界信道)。
2.  **消费者**: `AccessLogHostedService` 是一个 `BackgroundService`。
    *   它维护一个 `while(!token)` 循环。
    *   使用 `SemaphoreSlim` (`ShortUrlConfig.SemaphoreCount`) 来控制并发（虽然目前是单线程循环，主要用于潜在的扩展示例）。
    *   **批量消费**: 每次循环尝试从 Channel 读取 `LogBufferSize` (配置项，默认 20) 个日志，或者直到队列为空。
    *   **批量插入**: 调用 `repository.AddRangeAsync` 一次性写入 DB。

### 为什么这样做？
*   **削峰填谷**: 流量突发时，内存队列暂存压力，数据库始终以平稳的速率（批量）写入。
*   **减少 IO**: 100 次点击合并为 1 次 DB Insert 操作，极大降低数据库连接开销。

### 风险点
*   **数据丢失**: 如果服务突然 Crash（如 OOM 或断电），**内存队列中尚未入库的日志会丢失**。对于访问统计来说，这是可接受的 Trade-off（性能 > 绝对精度）。如果不接受，需要换成 Kafka 保证队列数据的可用性。

## 1.4. 缓存重试

*   **Proxy 模式**: 代码中使用了 `RedisRetryProxy`。不要直接使用 `StackExchange.Redis` 的原生连接。这个 Proxy 封装了 polly 重试策略（SleepDurations: 1s, 3s, 5s）。
*   **缓存更新**:
    *   生成短链时：立即写入 Redis。
    *   访问短链时：Cache Aside 模式（按需加载）。
    *   **注意**: 修改短链配置（如延长有效期）时，记得通过 `SetShortUrlCache` 更新 Redis，否则用户会因为缓存用到旧的过期时间。

---

项目的核心就在于 **"Base62 混淆算法"** 保证生成的唯一性和安全性，以及 **"Redis + Channel"** 组合拳来扛住高并发读取和写入。