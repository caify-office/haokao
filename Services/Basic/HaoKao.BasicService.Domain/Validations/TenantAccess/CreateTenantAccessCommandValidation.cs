using HaoKao.BasicService.Domain.Commands.TenantAccess;

namespace HaoKao.BasicService.Domain.Validations.TenantAccess;

public class CreateTenantAccessCommandValidation : TenantAccessCommandValidation<CreateTenantAccessCommand>
{
    public CreateTenantAccessCommandValidation()
    {
        ValidationCopyright();
        ValidationFavicon();
        ValidationIntroduction();
        ValidationLogo();
        ValidationAccessName();
        ValidationCopyrightAddress();
        ValidationFilingAddress();
        ValidationHttpAddress();
        ValidationIcpFiling();
        ValidationOrganizationalUnit();
        ValidationWebSiteName();
    }
}