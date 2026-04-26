namespace HaoKao.CorrectionNotebookService.Domain.Services;

public interface IImageDemoire : IDomainService
{
    /// <summary>
    /// 图像去莫尔
    /// </summary>
    /// <param name="imageBase64"></param>
    /// <returns></returns>
    Task<string> DemoireImage(string imageBase64);
}