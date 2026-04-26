using Girvs.Infrastructure;
using HaoKao.AnsweringQuestionService.Domain.Commands.AnsweringQuestionReply;
using HaoKao.AnsweringQuestionService.Domain.Entities;
using HaoKao.AnsweringQuestionService.Domain.Repositories;
using HaoKao.Common.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HaoKao.AnsweringQuestionService.Domain.CommandHandlers;

public class AnsweringQuestionReplyCommandHandler(
    IUnitOfWork<AnsweringQuestionReply> uow,
    IAnsweringQuestionRepository questionRepository,
    IAnsweringQuestionReplyRepository replyRepository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreateAnsweringQuestionReplyCommand, bool>,
    IRequestHandler<UpdateAnsweringQuestionReplyCommand, bool>,
    IRequestHandler<DeleteAnsweringQuestionReplyCommand, bool>
{
    private readonly IAnsweringQuestionRepository _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
    private readonly IAnsweringQuestionReplyRepository _replyRepository = replyRepository ?? throw new ArgumentNullException(nameof(replyRepository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateAnsweringQuestionReplyCommand request, CancellationToken cancellationToken)
    {
        var replyUserName = EngineContext.Current.ClaimManager.IdentityClaim.UserName;
        var answeringQuestionReply = new AnsweringQuestionReply
        {
            ReplyContent = request.ReplyContent,
            ReplyUserName = replyUserName,
            AnsweringQuestionId = request.AnsweringQuestionId
        };

        await _replyRepository.AddAsync(answeringQuestionReply);

        if (await Commit())
        {
            //更改回复状态
            await _questionRepository.UpdateIsReply(request.AnsweringQuestionId);

            await _bus.UpdateIdCacheEvent(answeringQuestionReply, answeringQuestionReply.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<AnsweringQuestionReply>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateAnsweringQuestionReplyCommand request, CancellationToken cancellationToken)
    {
        var answeringQuestionReply = await _replyRepository.GetByIdAsync(request.Id);
        if (answeringQuestionReply == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应答疑回复的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        answeringQuestionReply.ReplyContent = request.ReplyContent;
        answeringQuestionReply.AnsweringQuestionId = request.AnsweringQuestionId;

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(answeringQuestionReply, answeringQuestionReply.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<AnsweringQuestionReply>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteAnsweringQuestionReplyCommand request, CancellationToken cancellationToken)
    {
        var answeringQuestionReply = await _replyRepository.GetByIdAsync(request.Id);
        if (answeringQuestionReply == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应答疑回复的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _replyRepository.DeleteAsync(answeringQuestionReply);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<AnsweringQuestionReply>(answeringQuestionReply.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<AnsweringQuestionReply>(cancellationToken);
        }

        return true;
    }
}