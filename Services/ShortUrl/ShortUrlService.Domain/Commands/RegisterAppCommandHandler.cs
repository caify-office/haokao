using Microsoft.IdentityModel.Tokens;
using ShortUrlService.Domain.Entities;
using ShortUrlService.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShortUrlService.Domain.Commands;

public class RegisterAppCommandHandle(
    IUnitOfWork<RegisterApp> uow,
    IMediatorHandler bus,
    IRegisterAppRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateRegisterAppCommand, string>
{
    private readonly IRegisterAppRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<string> Handle(CreateRegisterAppCommand request, CancellationToken cancellationToken)
    {
        var entity = new RegisterApp
        {
            AppName = request.AppName,
            AppCode = request.AppCode,
            AppSecret = GenerateSecret(request.AppName, request.AppCode),
            Description = request.Description,
            AppDomains = [.. request.AppDomains,],
        };

        await _repository.AddAsync(entity);

        return await Commit() ? entity.AppSecret : throw new GirvsException("创建注册应用失败");
    }

    private static string GenerateSecret(string appName, string appCode)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, appName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
            new Claim("appCode", appCode),
        };

        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes($"{appName}:{appCode}"));
        var key = new SymmetricSecurityKey(bytes);
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            "ShortUrlService",
            appCode,
            claims,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}