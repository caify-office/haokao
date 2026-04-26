namespace HaoKao.QuestionService.Application.QuestionModule.Interfaces;

public interface IQuestionTypeService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取题型列表
    /// </summary>
    dynamic GetQuestionTypeList();

    /// <summary>
    /// 获取题型配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    dynamic GetQuestionTypeConfig(Guid id);

    /// <summary>
    /// 获取题型信息列表
    /// </summary>
    dynamic GetQuestionTypeInfoList();
}