using HaoKao.BasicService.Domain.Commands.TenantAccess;

namespace HaoKao.BasicService.Domain.Validations.TenantAccess;

public abstract class TenantAccessCommandValidation<TCommand> : GirvsCommandValidator<TCommand>
    where TCommand : TenantAccessCommand
{
    protected virtual void ValidationCopyright()
    {
        RuleFor(t => t.Copyright)
            .MaximumLength(100).WithMessage("版权所有长度不能大于100");
    }
        
    protected virtual void ValidationFavicon()
    {
        RuleFor(t => t.Favicon)
            .NotEmpty().WithMessage("图站图标不能为空")
            .MaximumLength(500).WithMessage("图站图标长度不能大于500");
    }
        
    protected virtual void ValidationIntroduction()
    {
        RuleFor(t => t.Introduction)
            .MaximumLength(1000).WithMessage("站点简介长度不能大于1000");
    }
        
    protected virtual void ValidationLogo()
    {
        RuleFor(t => t.Logo)
            .NotEmpty().WithMessage("图站Logo不能为空")
            .MaximumLength(500).WithMessage("图站Logo长度不能大于500");
    }
        
    protected virtual void ValidationAccessName()
    {
        RuleFor(t => t.AccessName)
            .NotEmpty().WithMessage("当前设置的名称不能为空")
            .MinimumLength(4).WithMessage("当前设置的名称长度不能小于4")
            .MaximumLength(50).WithMessage("当前设置的名称长度不能大于50");
    }
        
    protected virtual void ValidationCopyrightAddress()
    {
        RuleFor(t => t.CopyrightAddress)
            .MaximumLength(500).WithMessage("版权链接地址长度不能大于500");
    }
        
    protected virtual void ValidationFilingAddress()
    {
        RuleFor(t => t.FilingAddress)
            .MaximumLength(500).WithMessage("Icp备案地址链接地址长度不能大于500");
    }
        
    protected virtual void ValidationHttpAddress()
    {
        RuleFor(t => t.HttpAddress)
            .NotEmpty().WithMessage("Http访问地址不能为空")
            .MaximumLength(500).WithMessage("Http访问地址长度不能大于500");
    }
        
    protected virtual void ValidationIcpFiling()
    {
        RuleFor(t => t.IcpFiling)
            .MaximumLength(40).WithMessage("Icp 备案号长度不能大于40");
    }
        
    protected virtual void ValidationOrganizationalUnit()
    {
        RuleFor(t => t.OrganizationalUnit)
            .MaximumLength(40).WithMessage("组织单位长度不能大于40");
    }
        
    protected virtual void ValidationWebSiteName()
    {
        RuleFor(t => t.WebSiteName)
            .NotEmpty().WithMessage("网站名称不能为空")
            .MinimumLength(2).WithMessage("网站名称长度不能小于2")
            .MaximumLength(100).WithMessage("网站名称长度不能大于100");
    }
}