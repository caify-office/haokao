namespace HaoKao.CorrectionNotebookService.Domain.Services;

public interface IImageDewarp : IDomainService
{
    /// <summary>
    /// 图片畸变矫正
    /// </summary>
    /// <param name="imageBase64"></param>
    /// <returns></returns>
    Task<string> DewarpImage(string imageBase64);
}