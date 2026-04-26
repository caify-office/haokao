namespace HaoKao.QuestionService.Domain.QuestionModule;

public class QuestionType
{
    /// <summary>
    /// 单选
    /// </summary>
    public static Guid SingleChoice = new("0e4ec1ed-515b-4cfc-82b7-5248df822c02");

    /// <summary>
    /// 多选
    /// </summary>
    public static Guid MultiChoice = new("b93af157-de41-4f23-ab9f-378a43a133fe");

    /// <summary>
    /// 不定项
    /// </summary>
    public static Guid Undetermined = new("990f535d-9327-4b8c-908b-22d3b179de8f");

    /// <summary>
    /// 判断
    /// </summary>
    public static Guid Judgment = new("00fb75df-0f82-48e8-a65c-b2ab9c049f14");

    /// <summary>
    /// 填空
    /// </summary>
    public static Guid FillIn = new("239b2fe5-8eb1-4ff2-a8f7-f3ea96ebf2b0");

    /// <summary>
    /// 案例分析
    /// </summary>
    public static Guid CaseAnalysis = new("68eae47e-3df7-4c78-9e73-0294bcbdd7ac");

    /// <summary>
    /// 问答
    /// </summary>
    public static Guid ShortAnswer = new("e7fff3b0-655e-4d8c-a007-62e10c20e288");

    /// <summary>
    /// 所有题型
    /// </summary>
    public static Guid[] All =
    [
        SingleChoice,
        MultiChoice,
        Undetermined,
        Judgment,
        FillIn,
        CaseAnalysis,
        ShortAnswer,
    ];
}