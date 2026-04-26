namespace HaoKao.CorrectionNotebookService.Domain.Services;

public interface IOcrService : IDomainService
{
    Task<string> Scan(Uri imageUrl);
}