namespace HaoKao.CorrectionNotebookService.Domain.Services;

public interface ILargeLanguageModel : IDomainService
{
    string Name { get; }

    Task<string> CompletionAsync(string content);

    IAsyncEnumerable<string> CompletionStreamAsync(string content);

    string ReadStream(IReadOnlyList<string> chunks);
}