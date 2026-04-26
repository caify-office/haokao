using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Domain.Commands.ProductPackage;

/// <summary>
/// 创建产品包命令
/// </summary>
/// <param name="SimpleName">简称</param>
/// <param name="SalesRemind">售后提醒</param>
/// <param name="Name">名称</param>
/// <param name="ProductType">产品包类型</param>
/// <param name="CardImage">产品卡片图片</param>
/// <param name="DetailImage">详细介绍图片</param>
/// <param name="NumberOfBuyers">购买人数配置</param>
/// <param name="Selling">卖点</param>
/// <param name="ExpiryTime">到期时间</param>
/// <param name="PreferentialExpiryTime">优惠到期时间</param>
/// <param name="Year">所属年份</param>
/// <param name="FeaturedService">特色服务</param>
/// <param name="Hot">热门推荐</param>
/// <param name="Contrast">对比</param>
/// <param name="Enable">启用</param>
/// <param name="Shelves">上架</param>
/// <param name="Detail">对比详细介绍</param>
/// <param name="ProductList">产品列表</param>
/// <param name="LecturerList">讲师列表</param>
/// <param name="ComparisonDictionaryId">对比字典Id</param>
/// <param name="SimpleDesc">简单描述</param>
/// <param name="Desc">描述</param>
/// <param name="IsExperience">是否体验产品包</param>
/// <param name="IsSupportInstallmentPayment">是否支持分期支付</param>
/// <param name="Sort"></param>
public record CreateProductPackageCommand(
    string SimpleName,
    string SalesRemind,
    string Name,
    ProductType ProductType,
    string CardImage,
    string DetailImage,
    int NumberOfBuyers,
    List<string> Selling,
    DateTime ExpiryTime,
    DateTime PreferentialExpiryTime,
    DateTime Year,
    List<Guid> FeaturedService,
    bool Hot,
    bool Contrast,
    bool Enable,
    bool Shelves,
    Dictionary<Guid, string> Detail,
    List<Guid> ProductList,
    List<Guid> LecturerList,
    Guid? ComparisonDictionaryId,
    string SimpleDesc,
    string Desc,
    bool IsExperience,
    bool IsSupportInstallmentPayment,
    int Sort
) : Command("创建产品包")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("名称不能为空")
                 .MaximumLength(100).WithMessage("名称长度不能大于100")
                 .MinimumLength(2).WithMessage("名称长度不能小于2");

        validator.RuleFor(x => CardImage)
                 .NotEmpty().WithMessage("产品卡片图片不能为空")
                 .MaximumLength(1000).WithMessage("产品卡片图片长度不能大于1000")
                 .MinimumLength(2).WithMessage("产品卡片图片长度不能小于2");

        validator.RuleFor(x => DetailImage)
                 .NotEmpty().WithMessage("详细介绍图片不能为空")
                 .MaximumLength(1000).WithMessage("详细介绍图片长度不能大于1000")
                 .MinimumLength(2).WithMessage("详细介绍图片长度不能小于2");

        validator.RuleFor(x => Desc)
                 .MaximumLength(1000).WithMessage("描述长度不能大于1000");
    }
}