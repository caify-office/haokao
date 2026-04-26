namespace HaoKao.CorrectionNotebookService.Domain.Services;

public interface IImageEnhancement : IDomainService
{
    /// <summary>
    /// 图片增强
    /// </summary>
    /// <param name="imageBase64"></param>
    /// <returns></returns>
    Task<string> EnhanceImage(string imageBase64);
}