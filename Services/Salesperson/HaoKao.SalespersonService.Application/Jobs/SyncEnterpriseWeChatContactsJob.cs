using Girvs.Quartz;
using HaoKao.SalespersonService.Application.Services;
using HaoKao.SalespersonService.Domain.Entities;
using HaoKao.SalespersonService.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HaoKao.SalespersonService.Application.Jobs;

public class SyncEnterpriseWeChatContactsJob(
    IServiceProvider serviceProvider,
    ILogger<SyncEnterpriseWeChatContactsJob> logger
) : GirvsJob(serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    private readonly ILogger<SyncEnterpriseWeChatContactsJob> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public override async void GirvsExecute(IJobExecutionContext context)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var salespersonRepository = scope.ServiceProvider.GetRequiredService<ISalespersonRepository>();
        var contactRepository = scope.ServiceProvider.GetRequiredService<IEnterpriseWeChatContactRepository>();

        var salespersons = await salespersonRepository.GetIncludeAll();
        var wechat = new EnterpriseWeChatService();

        foreach (var salesperson in salespersons)
        {
            try
            {
                var accessToken = await wechat.GetAccessToken(salesperson.EnterpriseWeChatConfig.CorpId, salesperson.EnterpriseWeChatConfig.CorpSecret);
                var externalUsers = await wechat.GetExternalContactList(salesperson.EnterpriseWeChatUserId, accessToken.access_token);

                // 获取销售人员的微信联系人
                var localContacts = await contactRepository.GetByFollowUserId(salesperson.EnterpriseWeChatUserId);

                // 1. 如果数据库中有, 企业微信通讯录中没有, 则删除数据库中的销售人员
                var contactsForDelete = localContacts.Where(x => !externalUsers.external_userid.Contains(x.UserId)).ToList();
                if (contactsForDelete.Count > 0)
                {
                    await contactRepository.DeleteRangeAsync(contactsForDelete);
                }

                // 2. 如果数据库中没有, 企业微信通讯录中有, 则新增数据库中的销售人员
                var contactsForAdd = externalUsers.external_userid.Where(x => !localContacts.Any(y => y.UserId == x)).ToList();
                if (contactsForAdd.Count == 0) continue;

                // 并行拉取用户详情
                var chunks = SplitList(contactsForAdd, 32);
                foreach (var chunk in chunks)
                {
                    var list = new List<EnterpriseWeChatContact>(chunk.Count);
                    Task.WaitAll(chunk.Select(async userId =>
                    {
                        var x = await wechat.GetExternalContact(accessToken.access_token, userId);
                        list.Add(new EnterpriseWeChatContact
                        {
                            FollowUserId = salesperson.EnterpriseWeChatUserId,
                            FollowUserName = salesperson.EnterpriseWeChatUserName,
                            Name = x.external_contact.name,
                            Type = x.external_contact.type,
                            UserId = x.external_contact.external_userid,
                            UnionId = x.external_contact.unionid ?? "",
                            CreateTime = DateTime.Now,
                        });
                    }).ToArray());
                    await contactRepository.AddRangeAsync(list);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                continue;
            }
        }
    }

    private static List<List<string>> SplitList(List<string> list, int size)
    {
        var result = new List<List<string>>();
        for (int i = 0; i < list.Count; i += size)
        {
            var chunk = list.GetRange(i, Math.Min(size, list.Count - i));
            result.Add(chunk);
        }
        return result;
    }
}