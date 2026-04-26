using HaoKao.ContinuationService.Domain.ContinuationAuditModule;
using HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;
using HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;
using HaoKao.ContinuationService.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace HaoKao.ContinuationService.Infrastructure.Services;

public class LegacyDataMigrationService(
    ContinuationAuditRecordDbContext dbContext,
    ILogger<LegacyDataMigrationService> logger
) : ILegacyDataMigrationService
{
    private readonly ContinuationAuditRecordDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private readonly ILogger<LegacyDataMigrationService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<(int MigratedSetups, int MigratedAudits, int Errors)> MigrateAsync(CancellationToken cancellationToken = default)
    {
        var migratedSetups = 0;
        var migratedAudits = 0;
        var errors = 0;

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            // 1. 迁移 ContinuationSetup -> ProductExtensionPolicy
            var legacySetups = await _dbContext.ContinuationSetups.AsNoTracking().ToListAsync(cancellationToken);

            foreach (var setup in legacySetups)
            {
                // 检查是否已迁移 (假设通过 ID 相同来判断，如果 ID 策略不同则需调整)
                var existingPolicy = await _dbContext.ProductExtensionPolicies.FindAsync([setup.Id], cancellationToken);
                if (existingPolicy != null)
                {
                    _logger.LogInformation($"Setup {setup.Id} already migrated. Skipping.");
                    continue;
                }

                var policy = new ProductExtensionPolicy
                {
                    Id = setup.Id, // 保留原 ID
                    Name = "历史迁移策略", // 旧表无 Name，赋默认值
                    StartTime = setup.StartTime,
                    EndTime = setup.EndTime,
                    ExtensionType = ExtensionType.FixedDate, // 旧逻辑只有 ExpiryTime，对应 FixedDate
                    ExpiryDate = setup.ExpiryTime,
                    ExtensionDays = null,
                    IsEnable = setup.Enable,
                    CreateTime = setup.CreateTime,
                    UpdateTime = setup.UpdateTime,
                    TenantId = setup.TenantId,
                    IsDelete = setup.IsDelete,
                    Products = setup.Products.Select(p => new PolicyProduct
                    {
                        ProductId = p.ProductId,
                        AgreementId = p.AgreementId,
                        MaxExtensionCount = p.Continuation
                    }).ToList()
                };

                await _dbContext.ProductExtensionPolicies.AddAsync(policy, cancellationToken);
                migratedSetups++;
            }

            await _dbContext.SaveChangesAsync(cancellationToken); // 先保存策略，确保外键约束

            // 2. 迁移 ContinuationAudit -> ProductExtensionRequest
            var legacyAudits = await _dbContext.ContinuationAudits.AsNoTracking().ToListAsync(cancellationToken);

            foreach (var audit in legacyAudits)
            {
                var existingRequest = await _dbContext.ProductExtensionRequests.FindAsync([audit.Id], cancellationToken);
                if (existingRequest != null)
                {
                    _logger.LogInformation($"Audit {audit.Id} already migrated. Skipping.");
                    continue;
                }

                // 映射状态
                var newState = audit.AuditState switch
                {
                    AuditState.InAudit => ProductExtensionRequestState.Waiting,
                    AuditState.Pass => ProductExtensionRequestState.Approved,
                    AuditState.NotPass => ProductExtensionRequestState.Rejected,
                    _ => ProductExtensionRequestState.Waiting
                };

                var request = new ProductExtensionRequest
                {
                    Id = audit.Id, // 保留原 ID
                    PolicyId = audit.SetupId,
                    ProductId = audit.ProductId,
                    ProductName = audit.ProductName,
                    AgreementId = audit.AgreementId,
                    AgreementName = audit.AgreementName,
                    ExpiryTime = audit.ExpiryTime,
                    StudentName = audit.StudentName,
                    ReasonId = audit.Reason,
                    Description = audit.Description,
                    Evidences = audit.Evidences,
                    ProductGifts = audit.ProductGifts,
                    AuditState = newState,
                    AuditReason = audit.AuditReason,
                    AuditTime = audit.UpdateTime, // 旧表无 AuditTime，暂时用 UpdateTime 近似
                    AuditOperatorName = null, // 旧表无 OperatorName
                    RestOfCount = audit.RestOfCount,
                    TenantId = audit.TenantId,
                    CreateTime = audit.CreateTime,
                    UpdateTime = audit.UpdateTime,
                    CreatorId = audit.CreatorId,
                    AuditLogs = []
                };

                // 如果已审核，生成一条历史日志
                if (newState != ProductExtensionRequestState.Waiting)
                {
                    request.AuditLogs.Add(new ProductExtensionAuditLog
                    {
                        Id = Guid.NewGuid(),
                        RequestId = request.Id,
                        NewState = newState,
                        Remark = audit.AuditReason,
                        CreateTime = audit.UpdateTime, // 使用最后更新时间
                        CreatorId = Guid.Empty, // 未知操作人
                        CreatorName = "System Migration"
                    });
                }

                await _dbContext.ProductExtensionRequests.AddAsync(request, cancellationToken);
                migratedAudits++;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Error during legacy data migration");
            errors++;
            throw; // 重新抛出异常以便上层处理
        }

        return (migratedSetups, migratedAudits, errors);
    }
}