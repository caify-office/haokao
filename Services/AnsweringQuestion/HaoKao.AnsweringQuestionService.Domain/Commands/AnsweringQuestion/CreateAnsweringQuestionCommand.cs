using FluentValidation;
using HaoKao.AnsweringQuestionService.Domain.Enumerations;
using System;

namespace HaoKao.AnsweringQuestionService.Domain.Commands.AnsweringQuestion;
/// <summary>
/// 创建答疑命令
/// </summary>
/// <param name="ParentId">用户名称</param>
/// <param name="SubjectId">科目id</param>
/// <param name="SubjectName">科目名称</param>
/// <param name="CourseId">课程id</param>
/// <param name="CourseChapterId">选择的课程章节id</param>
/// <param name="CourseVideId">选择的课程视频id</param>
/// <param name="BookPageSize">书籍页码</param>
/// <param name="BookName">书籍名称以及相关描述</param>
/// <param name="Type">提问类型</param>
/// <param name="Description">问题描述</param>
/// <param name="Remark">详细描述</param>
/// <param name="FileUrl">上传的图片路劲</param>
/// <param name="CourseName">课程名称</param>
/// <param name="CourseChapterName">课程章节名称</param>
/// <param name="CourseVideoName">课程视频名称</param>
/// <param name="ProductId">产品id</param>
public record CreateAnsweringQuestionCommand(
    Guid? ParentId,
    Guid SubjectId,
    string SubjectName,
    Guid CourseId,
    Guid CourseChapterId,
    Guid CourseVideId,
    string BookPageSize,
    string BookName,
    AnsweringQuestionEnum Type,
    string Description,
    string Remark,
    string FileUrl,
    string CourseName,
    string CourseChapterName,
    string CourseVideoName,Guid ProductId
) : Command("创建答疑")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {



















    }
}