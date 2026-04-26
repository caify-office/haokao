// using Girvs.Grpc;
using Grpc.Core;
using ShortUrlService.WebApi.Grpc;
using static ShortUrlService.WebApi.Grpc.ShortUrlGrpcService;

namespace ShortUrlService.WebApi.Services;

public class ShortUrlGrpcService(IShortUrlService service) : ShortUrlGrpcServiceBase //, IAppGrpcService
{
    private readonly IShortUrlService _service = service ?? throw new ArgumentNullException(nameof(service));

    public override async Task<GenerateReply> Generate(GenerateRequest request, ServerCallContext context)
    {
        var response = await _service.GenerateAsync(
            new(
                request.OriginUrl,
                request.AccessLimit,
                request.ExpiredTime.ToDateTime(),
                request.AppCode, request.AppSecret
            )
        );

        return new GenerateReply
        {
            ShortKey = response.ShortKey,
            ShortUrl = response.ShortUrl.ToString(),
            QrCodeBase64 = response.QrCodeBase64
        };
    }
}