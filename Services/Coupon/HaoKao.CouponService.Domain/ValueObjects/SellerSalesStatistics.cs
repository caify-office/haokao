using System.Collections.Generic;
using System.Linq;

namespace HaoKao.CouponService.Domain.ValueObjects;

public class SellerSalesStatistics : ValueObject<SellerSalesStatistics>
{
    public SellerSalesStatistics() { }

    public SellerSalesStatistics(string personName, int totalCount, decimal totalAmount)
    {
        PersonName = personName;
        TotalCount = totalCount;
        TotalAmount = totalAmount;
    }

    // WARNING: 不能删除 init 否则 EF 查询会报错
    public string PersonName { get; init; }

    public int TotalCount { get; init; }

    public decimal TotalAmount { get; init; }

    protected override bool EqualsCore(SellerSalesStatistics other)
    {
        if (other == null || other.GetType() != GetType())
        {
            return false;
        }

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    protected override int GetHashCodeCore()
    {
        return GetEqualityComponents()
               .Select(x => x?.GetHashCode() ?? 0)
               .Aggregate((x, y) => x ^ y);
    }

    private IEnumerable<object> GetEqualityComponents()
    {
        yield return PersonName;
        yield return TotalCount;
        yield return TotalAmount;
    }
}