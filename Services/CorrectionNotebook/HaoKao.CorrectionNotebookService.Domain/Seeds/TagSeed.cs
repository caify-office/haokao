using HaoKao.CorrectionNotebookService.Domain.Entities;

namespace HaoKao.CorrectionNotebookService.Domain.Seeds;

public class TagSeed
{
    private static Tag Create(Guid id, string category, string name, DateTime dateTime) => new()
    {
        Id = id,
        Category = category,
        Name = name,
        IsBuiltIn = true,
        CreatorId = Guid.Empty,
        CreateTime = dateTime,
    };

    private static DateTime CreateTime => new(2024, 1, 1);

    public static IReadOnlyList<Tag> Data => [
        Create(new("0190fd34-7e0c-70ed-b554-cab99cf71a2b"), "题型", "单选题", CreateTime.AddSeconds(1)),
        Create(new("0190fd34-7e0c-7f7d-97a8-e6f31310ea90"), "题型", "多选题", CreateTime.AddSeconds(2)),
        Create(new("0190fd34-7e0c-7c19-ae1a-e069e46dad5a"), "题型", "不定项选择题", CreateTime.AddSeconds(3)),
        Create(new("0190fd34-7e0c-7c03-af63-0482f1f8177b"), "题型", "判断题", CreateTime.AddSeconds(4)),
        Create(new("0190fd34-7e0c-74cb-95fa-2204eb2217b1"), "题型", "填空题", CreateTime.AddSeconds(5)),
        Create(new("0190fd34-7e0c-76d4-92a6-5e2ee9b06a97"), "题型", "案例分析题", CreateTime.AddSeconds(6)),
        Create(new("0190fd34-7e0c-76c4-95dc-b85db92445d8"), "题型", "问答题", CreateTime.AddSeconds(7)),
        Create(new("0190fd34-7e0c-77a1-ba5c-6ea1e2d0d06b"), "题型", "其他", CreateTime.AddSeconds(8)),

        Create(new("0190fd34-7e0c-7927-8b80-e5c337fba29e"), "难易程度", "简单", CreateTime.AddSeconds(9)),
        Create(new("0190fd34-7e0c-7d4d-aef5-a7e00f35c5bb"), "难易程度", "一般", CreateTime.AddSeconds(10)),
        Create(new("0190fd34-7e0c-70b6-a3c9-7ea275f314a7"), "难易程度", "比较难", CreateTime.AddSeconds(11)),
        Create(new("0190fd34-7e0c-7a1c-9f34-6449080abdf3"), "难易程度", "非常难", CreateTime.AddSeconds(12)),

        Create(new("0190fd34-7e0c-7d33-87eb-fc7fdee7df7c"), "错误原因", "马虎粗心", CreateTime.AddSeconds(13)),
        Create(new("0190fd34-7e0c-7e98-84f5-c0cfbf5e27d1"), "错误原因", "概念未掌握", CreateTime.AddSeconds(14)),
        Create(new("0190fd34-7e0c-776d-8ecb-684b43b395c7"), "错误原因", "没有思路", CreateTime.AddSeconds(15)),
        Create(new("0190fd34-7e0c-7e34-8775-33f1dfa66fb7"), "错误原因", "运算错误", CreateTime.AddSeconds(16)),
        Create(new("0190fd34-7e0c-76b0-ad52-3b531e8a7756"), "错误原因", "其他", CreateTime.AddSeconds(17)),
    ];
}