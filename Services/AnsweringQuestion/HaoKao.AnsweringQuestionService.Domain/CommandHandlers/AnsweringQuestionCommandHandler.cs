using Girvs.Infrastructure;
using HaoKao.AnsweringQuestionService.Domain.Commands.AnsweringQuestion;
using HaoKao.AnsweringQuestionService.Domain.Entities;
using HaoKao.AnsweringQuestionService.Domain.Repositories;
using HaoKao.Common.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HaoKao.AnsweringQuestionService.Domain.CommandHandlers;

public class AnsweringQuestionCommandHandler(
    IUnitOfWork<AnsweringQuestion> uow,
    IAnsweringQuestionRepository repository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreateAnsweringQuestionCommand, bool>,
    IRequestHandler<UpdateAnsweringQuestionCommand, bool>,
    IRequestHandler<DeleteAnsweringQuestionCommand, bool>
{
    private readonly IAnsweringQuestionRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateAnsweringQuestionCommand request, CancellationToken cancellationToken)
    {
        var userName = EngineContext.Current.ClaimManager.IdentityClaim.UserName;
        var answeringQuestion = new AnsweringQuestion
        {
            ParentId = request.ParentId,
            UserName = userName,
            SubjectId = request.SubjectId,
            SubjectName = request.SubjectName,
            CourseId = request.CourseId,
            CourseChapterId = request.CourseChapterId,
            CourseVideId = request.CourseVideId,
            BookPageSize = request.BookPageSize,
            BookName = request.BookName,
            Type = request.Type,
            Description = request.Description,
            Remark = request.Remark,
            FileUrl = request.FileUrl,
            WatchCount = 0,
            IsReply = false,
            CourseName = request.CourseName,
            CourseChapterName = request.CourseChapterName,
            CourseVideName = request.CourseVideoName,
            ProductId = request.ProductId,
        };

        await _repository.AddAsync(answeringQuestion);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(answeringQuestion, answeringQuestion.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<AnsweringQuestion>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateAnsweringQuestionCommand request, CancellationToken cancellationToken)
    {
        var answeringQuestion = await _repository.GetByIdAsync(request.Id);
        if (answeringQuestion == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应答疑的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        answeringQuestion.WatchCount += 1;

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(answeringQuestion, answeringQuestion.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<AnsweringQuestion>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteAnsweringQuestionCommand request, CancellationToken cancellationToken)
    {
        var answeringQuestion = await _repository.GetByIdAsync(request.Id);
        if (answeringQuestion == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应答疑的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }
        //这里需要做一个判断  如果存在  子级数据 要先删除子级数据
        var children = await _repository.GetWhereAsync(w => w.ParentId == request.Id);
        if (children.Count > 0)
        {
            //先把子答疑删除
            await _repository.DeleteChildQuestion(request.Id);
        }
        await _repository.DeleteQuestion(request.Id);

        await _bus.RemoveIdCacheEvent<AnsweringQuestion>(answeringQuestion.Id.ToString(), cancellationToken);
        await _bus.RemoveListCacheEvent<AnsweringQuestion>(cancellationToken);
        return true;
    }
}