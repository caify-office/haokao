namespace HaoKao.CorrectionNotebookService.Application.Options;

public record EnabledServiceOptions : IAppModuleConfig
{
    public ServiceOptions OcrService { get; init; } = new(new("462c575b-047f-42dc-9e49-34e299137dfa"), "阿里云OCR");

    public ServiceOptions FreeLLMService { get; init; } = new(new("4dc0092d-dd20-48b1-ad8f-c54054aeed2d"), "通义千问");

    public ServiceOptions PaidLLMService { get; init; } = new(new("4d2324c6-83f8-4794-a95c-c27ae57b0da6"), "百度千帆");

    public ServiceOptions ImageDemoireService { get; init; } = new(new("01a53a77-c2b3-4ca5-97fa-57c90f991343"), "TextIn");

    public ServiceOptions ImageDewarpService { get; init; } = new(new("01a53a77-c2b3-4ca5-97fa-57c90f991343"), "TextIn");

    public ServiceOptions ImageEnhanceService { get; init; } = new(new("01a53a77-c2b3-4ca5-97fa-57c90f991343"), "TextIn");

    public ServiceOptions ImageRecognitionService { get; init; } = new(new("462c575b-047f-42dc-9e49-34e299137dfa"), "阿里云OCR");

    public void Init() { }
}

public record ServiceOptions(Guid Id, string Name);