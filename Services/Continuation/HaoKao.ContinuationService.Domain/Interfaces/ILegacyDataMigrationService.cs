using Girvs.BusinessBasis;

namespace HaoKao.ContinuationService.Domain.Interfaces;

public interface ILegacyDataMigrationService : IManager
{
    /// <summary>
    /// 迁移历史数据
    /// </summary>
    /// <returns>迁移结果统计信息 (迁移数量, 错误数量)</returns>
    Task<(int MigratedSetups, int MigratedAudits, int Errors)> MigrateAsync(CancellationToken cancellationToken = default);
}