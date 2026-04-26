using System.Collections.Concurrent;

namespace HaoKao.QuestionService.Application.QuestionHandlers;

public static class QuestionManager
{
    private static readonly ConcurrentDictionary<Guid, IQuestion> _questionDict = new();

    public static IQuestion GetByQuestionId(Guid id)
    {
        return _questionDict.GetOrAdd(id, key =>
        {
            var type = EngineContext.Current.Resolve<IEnumerable<IQuestion>>().FirstOrDefault(x => x.QuestionTypeId == key);
            if (type == null)
            {
                throw new GirvsException("错误的QuestionTypeId");
            }
            return type;
        });
    }
}