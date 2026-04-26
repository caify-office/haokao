using HaoKao.BasicService.Domain.Commands.User;
using HaoKao.BasicService.Domain.Entities;
using HaoKao.BasicService.Domain.Events;
using HaoKao.BasicService.Domain.Repositories;
using HaoKao.Common.Extensions;

namespace HaoKao.BasicService.Domain.CommandHandlers;

public class UserCommandHandler(
    IMediatorHandler bus,
    IUserRepository userRepository,
    IRepository<UserRule> userRuleRepository,
    IUnitOfWork<User> unitOfWork,
    IRoleRepository roleRepository
) : CommandHandler(unitOfWork, bus),
    IRequestHandler<CreateUserCommand, bool>,
    IRequestHandler<UpdateUserCommand, bool>,
    IRequestHandler<DeleteUserCommand, bool>,
    IRequestHandler<ChangeUserStateCommand, bool>,
    IRequestHandler<UpdateUserRoleCommand, bool>,
    IRequestHandler<ChangeUserPasswordCommand, bool>,
    IRequestHandler<AddUserRoleCommand, bool>,
    IRequestHandler<DeleteUserRoleCommand, bool>,
    IRequestHandler<UpdateUserRuleCommand, bool>,
    IRequestHandler<UserEditPasswordCommand, bool>,
    IRequestHandler<EventCreateUserCommand, bool>,
    IRequestHandler<EventEditUserCommand, bool>,
    IRequestHandler<BindContactNumberCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IRepository<UserRule> _userRuleRepository = userRuleRepository ?? throw new ArgumentNullException(nameof(userRuleRepository));
    private readonly IRoleRepository _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));

    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var tenantId = EngineContext.Current.ClaimManager.IdentityClaim.GetTenantId<Guid>();
        var existUser = await _userRepository.GetUserByLoginNameAndTenantIdAsync(request.UserAccount, tenantId);

        if (existUser != null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(nameof(request.UserAccount), $"用户账号{request.UserAccount}已存在"),
                cancellationToken);
            return false;
        }

        var user = new User
        {
            ContactNumber = request.ContactNumber,
            State = request.State,
            UserAccount = request.UserAccount,
            UserName = request.UserName,
            UserPassword = request.UserPassword,
            UserType = request.UserType,
            OtherId = request.OtherId,
            TenantId = tenantId,
            TenantName = EngineContext.Current.ClaimManager.IdentityClaim.TenantName
        };

        await _userRepository.AddAsync(user);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<User>(user.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<User>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        if (user.UserType is UserType.AdminUser or UserType.TenantAdminUser)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "当前用户禁止修改"),
                cancellationToken);
            return false;
        }

        user.ContactNumber = request.ContactNumber;
        user.State = request.State;
        user.UserName = request.UserName;
        user.UserType = request.UserType;

        if (user.UserPassword != request.UserPassword)
        {
            user.UserPassword = request.UserPassword;
        }

        await _userRepository.UpdateAsync(user);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<User>(user.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<User>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdIncludeRoleAndDataRule(request.Id);
        if (user == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        if (user.IsInitData)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "系统初始化数据，无法进行操作"),
                cancellationToken);
            return false;
        }

        if (user.UserType is UserType.AdminUser or UserType.TenantAdminUser)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "当前用户禁止修改"),
                cancellationToken);
            return false;
        }

        await _userRepository.DeleteAsync(user);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<User>(user.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<User>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(ChangeUserStateCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        if (user.UserType is UserType.AdminUser or UserType.TenantAdminUser)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "当前用户禁止修改"),
                cancellationToken);
            return false;
        }

        user.State = request.State;

        await _userRepository.UpdateAsync(user, nameof(User.State));

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<User>(user.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<User>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdIncludeRolesAsync(request.Id);

        if (user == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        var roles = await _roleRepository.GetWhereAsync(x => request.RoleIds.Contains(x.Id));
        user.Roles = roles ?? throw new ArgumentNullException(nameof(roles));

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<User>(user.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<User>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);

        if (user == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "用户不存在"), cancellationToken);
            return false;
        }

        user.UserPassword = request.NewPassword.ToMd5();

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<User>(user.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<User>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(AddUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdIncludeRolesAsync(request.UserId);
        if (user == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.UserId.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        var roles = await _roleRepository.GetWhereAsync(x => request.RoleIds.Contains(x.Id));
        user.Roles.AddRange(roles);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<User>(user.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<User>(cancellationToken);
            await _bus.RaiseEvent(new RemoveServiceCacheEvent(), cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdIncludeRolesAsync(request.UserId);
        if (user == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.UserId.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        user.Roles.RemoveAll(x => request.RoleIds.Contains(x.Id));

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<User>(user.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<User>(cancellationToken);
            await _bus.RaiseEvent(new RemoveServiceCacheEvent(), cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateUserRuleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdIncludeRoleAndDataRule(request.UserId);
        if (user == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.UserId.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _userRuleRepository.DeleteRangeAsync(user.RulesList);
        user.RulesList.Clear();
        user.RulesList.AddRange(request.UserRules);

        await _userRepository.UpdateAsync(user);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<User>(user.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<User>(cancellationToken);
            await _bus.RaiseEvent(new RemoveServiceCacheEvent(), cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UserEditPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);

        if (user == null || user.UserPassword != request.OldPassword.ToMd5())
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "用户不存在或原密码不正确"),
                cancellationToken);
            return false;
        }

        user.UserPassword = request.NewPassword.ToMd5();

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<User>(user.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<User>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(EventCreateUserCommand request, CancellationToken cancellationToken)
    {
        var existUser = await _userRepository.GetUserByLoginNameAndTenantIdAsync(request.UserAccount, request.TenantId);

        if (existUser != null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(nameof(request.UserAccount), "登陆名称已存在"),
                cancellationToken);
            return false;
        }

        var user = new User
        {
            ContactNumber = request.ContactNumber,
            State = request.State,
            UserAccount = request.UserAccount,
            UserName = request.UserName,
            UserPassword = request.UserPassword,
            UserType = request.UserType,
            OtherId = request.OtherId,
            TenantId = request.TenantId,
            TenantName = request.TenantName,
        };

        await _userRepository.CreateTenantIdAdmin(user);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<User>(user.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<User>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(EventEditUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByLoginNameAndTenantIdAsync(request.UserAccount, request.TenantId);

        if (user == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(nameof(request.UserAccount), "登陆名称不存在"),
                cancellationToken);
            return false;
        }

        user.TenantName = request.TenantName;
        user.UserName = request.UserName;
        user.UserPassword = string.IsNullOrEmpty(request.UserPassword) ? user.UserPassword : request.UserPassword;
        user.ContactNumber = request.ContactNumber;

        await _userRepository.EventUpdateAsync(user);

        // 修改当前租户下所有用户的考试名称
        var users = await _userRepository.GetUserByAndTenantIdAsync(request.TenantId.ToString());
        users.ForEach(x => { x.TenantName = request.TenantName; });
        await _userRepository.EventUpdateRangeAsync(users);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<User>(user.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<User>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(BindContactNumberCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        user.ContactNumber = request.ContactNumber;

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<User>(user.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<User>(cancellationToken);
            await _bus.RaiseEvent(new RemoveServiceCacheEvent(), cancellationToken);
        }

        return true;
    }
}