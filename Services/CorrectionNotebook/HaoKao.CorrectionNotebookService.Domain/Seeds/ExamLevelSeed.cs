using HaoKao.CorrectionNotebookService.Domain.Entities;

namespace HaoKao.CorrectionNotebookService.Domain.Seeds;

public class ExamLevelSeed
{
    private static ExamLevel Create(Guid id, string name, Guid parentId, DateTime dateTime) => new()
    {
        Id = id,
        Name = name,
        IsBuiltIn = true,
        CreatorId = Guid.Empty,
        CreateTime = dateTime,
        ParentId = parentId,
        Subjects = [],
    };

    private static DateTime CreateTime => new(2024, 1, 1);

    public static IReadOnlyList<ExamLevel> Data =>
    [
        Create(new("0190c3e6-3214-7dc6-bc25-6b0a5cab3d9e"), "经济师", Guid.Empty, CreateTime.AddSeconds(1)),
        Create(new("0190c3e7-a245-7983-997a-1141082743be"), "初级经济师", new("0190c3e6-3214-7dc6-bc25-6b0a5cab3d9e"), CreateTime.AddSeconds(2)),
        Create(new("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "中级经济师", new("0190c3e6-3214-7dc6-bc25-6b0a5cab3d9e"), CreateTime.AddSeconds(3)),
        Create(new("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "高级经济师", new("0190c3e6-3214-7dc6-bc25-6b0a5cab3d9e"), CreateTime.AddSeconds(4)),

        Create(new("0190c3e6-3214-735b-8d36-7b41ccd9918a"), "会计师", Guid.Empty, CreateTime.AddSeconds(5)),
        Create(new("0190c3e7-a245-7d10-9754-339536267635"), "注册会计师", new("0190c3e6-3214-735b-8d36-7b41ccd9918a"), CreateTime.AddSeconds(6)),
        Create(new("0190c3e7-a245-7efd-8d62-8a061fe71b22"), "中级会计职称", new("0190c3e6-3214-735b-8d36-7b41ccd9918a"), CreateTime.AddSeconds(7)),

        Create(new("0190c3e6-3214-7b8b-b2ae-0d578805c176"), "社会工作者", Guid.Empty, CreateTime.AddSeconds(8)),
        Create(new("0190c3e7-a245-7cb5-9613-122673d29071"), "初级社会工作者", new("0190c3e6-3214-7b8b-b2ae-0d578805c176"), CreateTime.AddSeconds(9)),
        Create(new("0190c3e7-a245-7b3c-b30f-4626b1f94b76"), "中级社会工作者", new("0190c3e6-3214-7b8b-b2ae-0d578805c176"), CreateTime.AddSeconds(10)),

        Create(new("0190c3e6-3214-776a-9f19-5bd81cf0b8c0"), "计算机软件资格", Guid.Empty, CreateTime.AddSeconds(11)),
        Create(new("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "软考中级职称", new("0190c3e6-3214-776a-9f19-5bd81cf0b8c0"), CreateTime.AddSeconds(12)),
        Create(new("0190c3e7-a245-7f93-ad52-3bc70bd9dcce"), "软考高级职称", new("0190c3e6-3214-776a-9f19-5bd81cf0b8c0"), CreateTime.AddSeconds(13)),
    ];
}