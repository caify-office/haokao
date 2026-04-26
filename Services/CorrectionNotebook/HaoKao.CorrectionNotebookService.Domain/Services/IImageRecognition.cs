namespace HaoKao.CorrectionNotebookService.Domain.Services;

public interface IImageRecognition : IDomainService
{
    /// <summary>
    /// 图像识别
    /// </summary>
    /// <param name="imageBase64"></param>
    /// <returns></returns>
    Task<string> RecognizeImage(string imageBase64);
}