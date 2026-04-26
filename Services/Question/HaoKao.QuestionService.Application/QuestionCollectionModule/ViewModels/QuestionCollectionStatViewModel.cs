namespace HaoKao.QuestionService.Application.QuestionCollectionModule.ViewModels;

public class QuestionCollectionStatViewModel : IDto
{
    public string TypeId { get; set; }

    public string TypeName { get; set; }

    public int Count { get; set; }
}