using HaoKao.BasicService.Domain.Commands.TenantAccess;

namespace HaoKao.BasicService.Domain.Validations.TenantAccess;

public class UpdateTenantAccessCommandValidation : TenantAccessCommandValidation<UpdateTenantAccessCommand>
{
    public UpdateTenantAccessCommandValidation()
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