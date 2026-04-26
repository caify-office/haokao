using HaoKao.Common.Enums;
using HaoKao.ProductService.Domain.Commands.ProductPermission;
using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Domain.Commands.Product;

/// <summary>
/// 更新产品
/// </summary>
/// <param name="Id">Id</param>
/// <param name="Name">产品名称</param>
/// <param name="DisplayName">显示名称</param>
/// <param name="ProductType">产品类别</param>
/// <param name="IsShelves">是否上架</param>
/// <param name="Description">产品描述</param>
/// <param name="Icon">产品图片</param>
/// <param name="DetailImage">产品详情图片</param>
/// <param name="ExpiryTimeTypeEnum">有效期类型</param>
/// <param name="Days">按天数</param>
/// <param name="ExpiryTime">到期时间</param>
/// <param name="Year">年份</param>
/// <param name="Price">价格</param>
/// <param name="DiscountedPrice">优惠价格</param>
/// <param name="AppleProductId">苹果内购产品ID</param>
/// <param name="Agreement">产品协议</param>
/// <param name="GiveAwayAList">赠送列表</param>
/// <param name="ProductPermissions">对应的权限列表</param>
/// <param name="AssistantProductPermissions">智辅产品权限列表</param>
/// <param name="IsExperience">是否体验产品</param>
public record UpdateProductCommand(
    Guid Id,
    string Name,
    string DisplayName,
    ProductType ProductType,
    bool IsShelves,
    string Description,
    string Icon,
    string DetailImage,
    ExpiryTimeTypeEnum ExpiryTimeTypeEnum,
    int Days,
    DateTime ExpiryTime,
    string Year,
    double Price,
    double DiscountedPrice,
    string AppleProductId,
    Guid? Agreement,
    Dictionary<Guid, string> GiveAwayAList,
    ICollection<ProductPermissionCommand> ProductPermissions,
    ICollection<AssistantProductPermissionCommand> AssistantProductPermissions,
    bool IsExperience
) : Command("更新产品")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("产品名称不能为空")
                 .MaximumLength(100).WithMessage("产品名称长度不能大于100")
                 .MinimumLength(2).WithMessage("产品名称长度不能小于2");

        validator.RuleFor(x => DisplayName)
                 .NotEmpty().WithMessage("显示名称不能为空")
                 .MaximumLength(100).WithMessage("显示名称长度不能大于100")
                 .MinimumLength(2).WithMessage("显示名称长度不能小于2");

        validator.RuleFor(x => Description)
                 .MaximumLength(2000).WithMessage("产品描述长度不能大于2000");

        validator.RuleFor(x => Icon)
                 .MaximumLength(1000).WithMessage("产品图标图片长度不能大于1000");

        validator.RuleFor(x => DetailImage)
                 .MaximumLength(1000).WithMessage("产品详情介绍图片长度不能大于1000");

        validator.RuleFor(x => AppleProductId)
                 .NotEmpty().WithMessage("苹果内购产品ID不能为空")
                 .MaximumLength(36).WithMessage("苹果内购产品ID长度不能大于36")
                 .MinimumLength(2).WithMessage("苹果内购产品ID长度不能小于2");
    }
}