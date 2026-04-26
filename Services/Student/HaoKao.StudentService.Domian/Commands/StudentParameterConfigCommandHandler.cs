using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Repositories;

namespace HaoKao.StudentService.Domain.Commands;

public class StudentParameterConfigCommandHandler(
    IUnitOfWork<StudentParameterConfig> uow,
    IMediatorHandler bus,
    IMapper mapper,
    IStudentParameterConfigRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateStudentParameterConfigCommand, bool>,
    IRequestHandler<UpdateStudentParameterConfigCommand, bool>,
    IRequestHandler<SaveStudentParameterConfigCommand, bool>,
    IRequestHandler<DeleteStudentParameterConfigCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IStudentParameterConfigRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(CreateStudentParameterConfigCommand request, CancellationToken cancellationToken)
    {
        var exist = await _repository.ExistEntityAsync(w => w.UserId == request.UserId
                                                         && w.PropertyType == request.PropertyType
                                                         && w.PropertyName == request.PropertyName);
        if (exist)
        {
            await _bus.RaiseEvent(
                new DomainNotification(
                    $"{request.UserId}-{request.PropertyName}",
                    "已存在对应学员对应字段类型对应字段名称的参数设置", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        var entity = _mapper.Map<StudentParameterConfig>(request);

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            await _bus.UpdateEntityCache(entity, cancellationToken);
            await _bus.RemoveListCache<StudentParameterConfig>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateStudentParameterConfigCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应学员参数设置的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        var exist = await _repository.ExistEntityAsync(w => w.UserId == request.UserId
                                                         && w.PropertyType == request.PropertyType
                                                         && w.PropertyName == request.PropertyName);
        if (exist)
        {
            await _bus.RaiseEvent(
                new DomainNotification(
                    $"{request.UserId}-{request.PropertyName}",
                    "已存在对应学员对应字段类型对应字段名称的参数设置", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        entity = _mapper.Map(request, entity);

        if (await Commit())
        {
            await _bus.UpdateEntityCache(entity, cancellationToken);
            await _bus.RemoveListCache<StudentParameterConfig>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(SaveStudentParameterConfigCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(w => w.UserId == request.UserId
                                                  && w.PropertyType == request.PropertyType
                                                  && w.PropertyName == request.PropertyName);
        if (entity == null)
        {
            entity = _mapper.Map<StudentParameterConfig>(request);
            await _repository.AddAsync(entity);
        }
        else
        {
            entity = _mapper.Map(request, entity);
            await _repository.UpdateAsync(entity);
        }

        if (await Commit())
        {
            await _bus.UpdateEntityCache(entity, cancellationToken);
            await _bus.RemoveListCache<StudentParameterConfig>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteStudentParameterConfigCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应学员参数设置的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(entity);

        if (await Commit())
        {
            await _bus.RemoveEntityCache<StudentParameterConfig>(request.Id, cancellationToken);
            await _bus.RemoveListCache<StudentParameterConfig>(cancellationToken);
        }

        return true;
    }
}