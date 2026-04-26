namespace HaoKao.FeedBackService.Domain.Commands.FeedBackReply;
/// <summary>
/// 创建答疑回复命令
/// </summary>
/// <param name="ReplyContent">答疑回复内容</param>
/// <param name="ReplyUserName">回复人用户名</param>
/// <param name="FeedBackId">关联的题目id</param>
/// <param name="FileUrl">上传的文件地址</param>
public record CreateFeedBackReplyCommand(
    string ReplyContent,
    string ReplyUserName,
    Guid FeedBackId,
    string FileUrl
) : Command("创建答疑回复")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {

        validator.RuleFor(x => ReplyContent)
            .NotEmpty().WithMessage("答疑回复内容不能为空");


        validator.RuleFor(x => ReplyUserName)
            .NotEmpty().WithMessage("回复人用户名不能为空");
        


    }
}