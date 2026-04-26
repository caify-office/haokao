namespace HaoKao.CorrectionNotebookService.Domain.Services;

public interface IDomainService
{
    Guid Id { get; }
}

public interface IServiceFactory<out T> where T : IDomainService
{
    T Create(Guid id);
}