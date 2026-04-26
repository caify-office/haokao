using Girvs.Infrastructure;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using HaoKao.Common.Extensions;
using System.Runtime.CompilerServices;

namespace HaoKao.ProductService.Infrastructure.Repositories;

public class StudentPermissionRepository(ProductDbContext dbContext) : Repository<StudentPermission>, IStudentPermissionRepository
{
    public Task<StudentPermission> GetWhereInclude(Guid userId, List<Guid> ids)
    {
        return Queryable.Where(x => ids.Contains(x.ProductId) && x.StudentId == userId)
                        .OrderBy(x => x.CreateTime)
                        .FirstOrDefaultAsync();
    }

    public Task<List<Guid>> GetProductIdsBy(Guid? subjectId, Guid? categoryId)
    {
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId()?.ToGuid();
        var query = dbContext.ProductPermissions.Where(x => x.SubjectId == subjectId && x.TenantId == tenantId);
        if (categoryId.HasValue)
        {
            query = query.Where(x => x.PermissionId == categoryId);
        }
        return query.Select(x => x.ProductId).ToListAsync();
    }

    /// <summary>
    ///  获取当前用户买过的产品id,名称和对应的协议id
    /// </summary>
    /// <returns></returns>
    public async Task<List<Tuple<Guid, string, Guid>>> GetAllAgreementId(Guid userId)
    {
        var query = dbContext.Products.Where(x => x.Agreement != Guid.Empty && x.Agreement != null);

        var tenantId = EngineContext.Current.ClaimManager.GetTenantId()?.ToGuid();
        if (tenantId.HasValue)
        {
            query = query.Where(x => x.TenantId == tenantId.Value);
        }

        var productIds = from s in dbContext.StudentPermissions
                         where s.StudentId == userId
                         select s.ProductId;

        var ids = from p in query
                  where productIds.Contains(p.Id)
                  select new Tuple<Guid, string, Guid>
                  (
                      p.Id,
                      p.Name,
                      p.Agreement.Value
                  );
        return await ids.ToListAsync();
    }

    /// <summary>
    /// 统计产品的购买人数
    /// </summary>
    /// <returns></returns>
    public Task<Dictionary<Guid, int>> GetAllProductBuyerCount()
    {
        var query = Queryable.Where(x => x.SourceMode == Domain.Enums.SourceMode.Purchase);

        //计算每个产品的购买人数(同一个人同一个产品购买两次，按一次计算)
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId()?.ToGuid();
        if (tenantId.HasValue && tenantId.Value != Guid.Empty)
        {
            query = query.Where(x => x.TenantId == tenantId);
        }

        return query.GroupBy(x => new { x.StudentId, x.ProductId })
                    .Select(g => new { g.Key.ProductId, g.Key.StudentId })
                    .GroupBy(x => x.ProductId)
                    .ToDictionaryAsync(x => x.Key, x => x.Count());
    }

    public async Task<IReadOnlyList<StudentPermission>> GetEnabledAndNotExpiredByUserId(Guid userId)
    {
        return await Queryable.AsNoTracking().Where(x => x.StudentId == userId && x.Enable && x.ExpiryTime >= DateTime.Now).ToListAsync();
    }

    public async Task SplitDataToShareTable()
    {
        var serviceProvider = EngineContext.Current.Resolve<IServiceProvider>();

        await using var tenantScope = serviceProvider.CreateAsyncScope();
        await using var tenantDbContextQuery = tenantScope.ServiceProvider.GetRequiredService<ProductDbContext>();

        //获取主表所有租户id
        var tenantIdList = tenantDbContextQuery.StudentPermissions.Select(x => x.TenantId).Distinct().ToList();
        //构建分表
        foreach (var tenantId in tenantIdList)
        {
            EngineContext.Current.ClaimManager.SetFromDictionary(new() { { GirvsIdentityClaimTypes.TenantId, tenantId.ToString() }, });
            await using var tenantScope1 = serviceProvider.CreateAsyncScope();
            await using var tenantDbContext = tenantScope1.ServiceProvider.GetRequiredService<ProductDbContext>();
            tenantDbContext.ShardingAutoMigration();
        }

        //获取分表名称
        var tables = await tenantDbContextQuery.GetTableNameList(nameof(StudentPermission));

        foreach (var table in tables)
        {
            var tenantId = Guid.Parse(table.Split('_')[1].Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(23, "-")).ToString();
            //将主表数据合入分表
            await using var tenantScope2 = serviceProvider.CreateAsyncScope();
            await using var tenantDbContextSp = tenantScope2.ServiceProvider.GetRequiredService<ProductDbContext>();
            var insertSql = CreateSql();
            await tenantDbContextSp.Database.ExecuteSqlAsync(FormattableStringFactory.Create($"{insertSql}"));
            continue;

            string CreateSql()
            {
                return @$"INSERT INTO {table} (Id,StudentName,StudentId,OrderNumber,SourceMode,ProductId,ProductName,ProductType,ExpiryTime,`Enable`,TenantId,CreateTime)
                SELECT Id,StudentName,StudentId,OrderNumber,SourceMode,ProductId,ProductName,ProductType,ExpiryTime,`Enable`,TenantId,CreateTime
                FROM `StudentPermission`
                WHERE TenantId='{tenantId}' AND  Id NOT IN(SELECT Id from {table})";
            }
        }
    }
}