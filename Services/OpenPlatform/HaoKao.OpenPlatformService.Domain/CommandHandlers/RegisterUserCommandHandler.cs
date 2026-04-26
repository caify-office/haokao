using Girvs;
using HaoKao.Common.Extensions;

namespace HaoKao.OpenPlatformService.Domain.CommandHandlers;

public class RegisterUserCommandHandler(
    IUnitOfWork<RegisterUser> uow,
    IMediatorHandler bus,
    IRegisterUserRepository repository,
    IRepository<ExternalIdentity> externalIdentityRepository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateRegisterUserCommand, bool>,
    IRequestHandler<UpdateRegisterUserCommand, bool>,
    IRequestHandler<DeleteRegisterUserCommand, bool>,
    IRequestHandler<BindExternalUserCommand, bool>,
    IRequestHandler<UpdateLastLoginCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateRegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await repository.ExistEntityAsync(x => x.Phone == request.Phone))
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Phone, "当前手机号码已注册", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

#pragma warning disable CS0618 // 类型或成员已过时
        var registerUser = new RegisterUser
        {
            Phone = request.Phone,
            Account = request.Phone,
            NickName = request.NickName ?? $"学员_{new SecureRandomNumberGenerator().Next(10000000, 99999999)}",
            Password = request.Phone.ToMd5(),
            UserGender = UserGender.Unknown,
            UserState = UserState.Enable,
            ClientId = request.ClientId,
        };
#pragma warning restore CS0618 // 类型或成员已过时

        if (request.CreatorId.HasValue && request.CreatorId != Guid.Empty)
        {
            registerUser.Id = request.CreatorId.Value;
        }

        if (request.ExternalUserCommand != null)
        {
            registerUser.NickName = request.ExternalUserCommand.NickName ?? registerUser.NickName;
            registerUser.HeadImage = request.ExternalUserCommand.HeadImage;
            registerUser.EmailAddress = request.ExternalUserCommand.EmailAddress;
        }

        if (request.ExternalUserCommand != null)
        {
            registerUser.ExternalIdentities.Add(new ExternalIdentity
            {
                Scheme = request.ExternalUserCommand.Scheme,
                UniqueIdentifier = request.ExternalUserCommand.UniqueIdentifier,
                OtherInformation = request.ExternalUserCommand.OtherInformation,
            });
        }

        await repository.AddAsync(registerUser);

        if (await Commit())
        {
            await ActionCache(registerUser, cancellationToken);
        }

        return true;
    }

    private async Task ActionCache(RegisterUser registerUser, CancellationToken cancellationToken = default)
    {
        // 创建缓存Key
        var idKey = GirvsEntityCacheDefaults<RegisterUser>.ByIdCacheKey.Create(registerUser.Id.ToString());
        // 将新增的纪录放到缓存中
        await _bus.RaiseEvent(new SetCacheEvent(registerUser, idKey, idKey.CacheTime), cancellationToken);

        // 创建缓存Key
        var phoneKey = GirvsEntityCacheDefaults<RegisterUser>.ByIdCacheKey.Create(registerUser.Phone);
        // 将新增的纪录放到缓存中
        await _bus.RaiseEvent(new SetCacheEvent(registerUser, phoneKey, phoneKey.CacheTime), cancellationToken);
        // 删除查询相关的缓存
        await _bus.RemoveListCacheEvent<RegisterUser>(cancellationToken);
    }

    public async Task<bool> Handle(UpdateRegisterUserCommand request, CancellationToken cancellationToken)
    {
        var registerUser = await repository.GetByInclude(w => w.Id.Equals(request.Id));
        if (registerUser == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应注册用户的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        registerUser.Phone = request.Phone;
        registerUser.Password = request.Password?.ToMd5() ?? registerUser.Password;
        registerUser.UserGender = request.UserGender;
        registerUser.NickName = request.NickName;
        registerUser.EmailAddress = request.EmailAddress;
        registerUser.UserState = request.UserState;
        registerUser.HeadImage = request.HeadImage;

        if (await Commit())
        {
            await ActionCache(registerUser, cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteRegisterUserCommand request, CancellationToken cancellationToken)
    {
        var registerUser = await repository.GetByInclude(w => w.Id.Equals(request.Id));
        if (registerUser == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应注册用户的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await externalIdentityRepository.DeleteRangeAsync(registerUser.ExternalIdentities);
        await repository.DeleteAsync(registerUser);

        if (await Commit())
        {
            var idKey = GirvsEntityCacheDefaults<RegisterUser>.ByIdCacheKey.Create(registerUser.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(idKey), cancellationToken);

            var phoneKey = GirvsEntityCacheDefaults<RegisterUser>.ByIdCacheKey.Create(registerUser.Phone);
            await _bus.RaiseEvent(new RemoveCacheEvent(phoneKey), cancellationToken);
           await _bus.RemoveListCacheEvent<RegisterUser>(cancellationToken);
        }

        return true;
    }

    private static string GetIpAddress()
    {
        try
        {
            return EngineContext.Current.HttpContext.Request.Headers["X-Forwarded-For"].ToString();
        }
        catch
        {
            return "localhost";
        }
    }

    public async Task<bool> Handle(BindExternalUserCommand request, CancellationToken cancellationToken)
    {
        var registerUser = await repository.GetByInclude(x => x.Phone == request.Phone);
        if (registerUser == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Phone, "未找到对应的用户", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        if (registerUser.ExternalIdentities.Any(x => x.Scheme == request.ExternalUserCommand.Scheme))
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Phone, "当前用户已被其它用户绑定", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        if (await externalIdentityRepository.ExistEntityAsync(x =>
            x.Scheme == request.ExternalUserCommand.Scheme &&
            x.UniqueIdentifier == request.ExternalUserCommand.UniqueIdentifier))
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Phone, "当前微信已被其它用户绑定", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        if (registerUser.NickName.IsNullOrEmpty())
        {
            registerUser.NickName = request.ExternalUserCommand.NickName;
        }

        if (registerUser.HeadImage.IsNullOrEmpty())
        {
            registerUser.HeadImage = request.ExternalUserCommand.HeadImage;
        }

        if (registerUser.EmailAddress.IsNullOrEmpty())
        {
            registerUser.EmailAddress = request.ExternalUserCommand.EmailAddress;
        }

        registerUser.ExternalIdentities.Add(new ExternalIdentity
        {
            Scheme = request.ExternalUserCommand.Scheme,
            UniqueIdentifier = request.ExternalUserCommand.UniqueIdentifier,
            OtherInformation = request.ExternalUserCommand.OtherInformation,
        });

        if (await Commit())
        {
            await ActionCache(registerUser, cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateLastLoginCommand request, CancellationToken cancellationToken)
    {
        var registerUser = await repository.GetByInclude(x => x.Id == request.Id);
        if (registerUser == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的用户", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        registerUser.LastLoginIp = GetIpAddress();
        registerUser.LastLoginTime = DateTime.Now;

        if (await Commit())
        {
            await ActionCache(registerUser, cancellationToken);
        }

        return true;
    }
}