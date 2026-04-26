using HaoKao.BasicService.Domain.Commands.Role;
using HaoKao.BasicService.Domain.Entities;
using HaoKao.BasicService.Domain.Events;
using HaoKao.BasicService.Domain.Repositories;
using HaoKao.Common.Extensions;

namespace HaoKao.BasicService.Domain.CommandHandlers;

public class RoleCommandHandler(
    IRoleRepository roleRepository,
    IMediatorHandler bus,
    IUserRepository userRepository,
    IUnitOfWork<Role> unitOfWork
) : CommandHandler(unitOfWork, bus),
    IRequestHandler<CreateRoleCommand, bool>,
    IRequestHandler<UpdateRoleCommand, bool>,
    IRequestHandler<DeleteRoleCommand, bool>,
    IRequestHandler<UpdateRoleUserCommand, bool>,
    IRequestHandler<AddRoleUserCommand, bool>,
    IRequestHandler<DeleteRoleUserCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IRoleRepository _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

    public async Task<bool> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Role
        {
            Name = request.Name,
            Desc = request.Desc,
            Users = request.UserIds.Select(uid => new User { Id = uid }).ToList()
        };

        await _roleRepository.AddAsync(role);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<Role>(role.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Role>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id);
        if (role == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据"),
                cancellationToken);
            return false;
        }

        role.Name = request.Name;
        role.Desc = request.Desc;

        // 移除不存在的用户
        role.Users.RemoveAll(user => !request.UserIds.Contains(user.Id));

        // 添加新的用户
        var newUserRoles = request.UserIds.Select(uId => new User { Id = uId }).ToList();
        newUserRoles.RemoveAll(userRole => role.Users.Select(o => o.Id).Contains(userRole.Id));
        role.Users.AddRange(newUserRoles);

        await _roleRepository.UpdateAsync(role);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<Role>(role.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Role>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id);
        if (role == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据"),
                cancellationToken);
            return false;
        }

        if (role.IsInitData)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "系统初始化数据，无法进行操作"),
                cancellationToken);
            return false;
        }

        await _roleRepository.DeleteAsync(role);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<Role>(role.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Role>(cancellationToken);
            await _bus.RaiseEvent(new RemoveServiceCacheEvent(), cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateRoleUserCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetRoleByIdIncludeUsersAsync(request.Id);
        role.Users.RemoveAll(x => !request.UserIds.Contains(x.Id));

        var userRoles = request.UserIds
                               .Where(userId => !role.Users.Select(userRole => userRole.Id).Contains(userId))
                               .Select(_ => new User { Id = request.Id }).ToList();

        if (userRoles.Count > 0)
        {
            role.Users.AddRange(userRoles);
        }

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<Role>(role.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Role>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(AddRoleUserCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetRoleByIdIncludeUsersAsync(request.RoleId);
        if (role == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.UserIds.ToString(), "未找到对应的数据"),
                cancellationToken);
            return false;
        }

        var users = await _userRepository.GetWhereAsync(x => request.UserIds.Contains(x.Id));
        role.Users.AddRange(users);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<Role>(role.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Role>(cancellationToken);
            await _bus.RaiseEvent(new RemoveServiceCacheEvent(), cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteRoleUserCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetRoleByIdIncludeUsersAsync(request.RoleId);
        if (role == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.UserIds.ToString(), "未找到对应的数据"),
                cancellationToken);
            return false;
        }

        role.Users.RemoveAll(x => request.UserIds.Contains(x.Id));

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<Role>(role.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Role>(cancellationToken);
            await _bus.RaiseEvent(new RemoveServiceCacheEvent(), cancellationToken);
        }

        return true;
    }
}