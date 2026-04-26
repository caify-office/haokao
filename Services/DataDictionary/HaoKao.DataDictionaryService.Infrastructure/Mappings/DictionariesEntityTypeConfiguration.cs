using HaoKao.DataDictionaryService.Domain.Entities;

namespace HaoKao.DataDictionaryService.Infrastructure.Mappings;

public class DictionariesEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Dictionaries>
{
    public override void Configure(EntityTypeBuilder<Dictionaries> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Dictionaries, Guid>(builder);
        builder.Property(x => x.Sort).HasColumnType("int");
        builder.Property(x => x.Code).HasColumnType("varchar(30)");
        builder.Property(x => x.Name).HasColumnType("varchar(225)");
        builder.Property(x => x.Pid).HasColumnType("char(36)");
        builder.Property(x => x.PName).HasColumnType("varchar(20)");
        builder.Property(x => x.State).HasColumnType("bit");
        builder.Property(x => x.TenantId).HasColumnType("char(36)");

        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => new { x.Code, x.Name, x.TenantId }).IsUnique();
        builder.HasIndex(x => x.Pid);
        builder.HasIndex(x => x.State);

        builder.HasData(_examLevel);
        builder.HasData(_courseComparison);
        builder.HasData(_continueReason);
        builder.HasData(_courseStage);
        builder.HasData(_hotSpotType);
        builder.HasData(_articleColumn);
        builder.HasData(_abilityDimension);
        builder.HasData(_expressProvince);
        builder.HasData(_paperTag);
    }

    #region 考试级别

    private readonly Dictionaries[] _examLevel =
    [
        CreateDictionary("08db395d-aeee-47d5-88eb-8a40315e1aad", "ZY8239", "考试级别", null, "顶级节点", "考试级别"),
        CreateDictionary("08db395d-b5db-4dc5-888c-a73d184bc184", "JJ9930", "经济师", "08db395d-aeee-47d5-88eb-8a40315e1aad", "考试级别", "考试级别=>经济师"),
        CreateDictionary("08db395d-bc27-4d09-86b5-c05dff7deb30", "XF9930", "消防师", "08db395d-aeee-47d5-88eb-8a40315e1aad", "考试级别", "考试级别=>消防师"),
        CreateDictionary("08db395d-c06c-4875-86da-987b25dba9db", "KJ9930", "会计师", "08db395d-aeee-47d5-88eb-8a40315e1aad", "考试级别", "考试级别=>会计师"),
    ];

    #endregion

    #region 课程对比

    private readonly Dictionaries[] _courseComparison =
    [
        CreateDictionary("08db9271-efed-48e6-8f86-6604089a8b6e", "KC0415", "课程对比", null, "顶级节点", "课程对比"),
        CreateDictionary("08db9271-fb47-47e0-8794-481bc7a83aac", "KG6752", "适合人群", "08db9271-efed-48e6-8f86-6604089a8b6e", "课程对比", "课程对比=>适合人群", 1),
        CreateDictionary("08db9271-fef4-4d75-8b1d-daf9ac8bb250", "JX7250", "教学模块", "08db9271-efed-48e6-8f86-6604089a8b6e", "课程对比", "课程对比=>教学模块", 2),
        CreateDictionary("08db9272-624c-4b91-8262-b063d4052f84", "JC9970", "基础阶段", "08db9271-fef4-4d75-8b1d-daf9ac8bb250", "教学模块", "课程对比=>教学模块=>基础阶段", 1),
        CreateDictionary("08db9272-651e-47bd-8b73-658fbc71f2d3", "JH1096", "强化阶段", "08db9271-fef4-4d75-8b1d-daf9ac8bb250", "教学模块", "课程对比=>教学模块=>强化阶段", 2),
        CreateDictionary("08db9272-6790-4200-80c2-b56431104573", "CC9246", "冲刺阶段", "08db9271-fef4-4d75-8b1d-daf9ac8bb250", "教学模块", "课程对比=>教学模块=>冲刺阶段", 3),
        CreateDictionary("08db9272-6ac8-4707-83f1-f827f086f932", "PS3711", "评审阶段", "08db9271-fef4-4d75-8b1d-daf9ac8bb250", "教学模块", "课程对比=>教学模块=>评审阶段", 4),
        CreateDictionary("08db9272-02e9-40f2-8627-5de8abd96dbb", "ZX1641", "助学模块", "08db9271-efed-48e6-8f86-6604089a8b6e", "课程对比", "课程对比=>助学模块", 3),
        CreateDictionary("08db9272-738b-436d-8502-29575be4bd4a", "BZ6107", "班主任督学", "08db9272-02e9-40f2-8627-5de8abd96dbb", "助学模块", "课程对比=>助学模块=>班主任督学", 1),
        CreateDictionary("08db9272-76e6-4071-890f-1417fcb49981", "ZX3244", "在线题库", "08db9272-02e9-40f2-8627-5de8abd96dbb", "助学模块", "课程对比=>助学模块=>在线题库", 2),
        CreateDictionary("08db9272-79e9-4541-8976-4b64ed4478f1", "FZ8704", "仿真模考测评", "08db9272-02e9-40f2-8627-5de8abd96dbb", "助学模块", "课程对比=>助学模块=>仿真模考测评", 3),
        CreateDictionary("08db9272-05ea-4d4e-8466-b7ceaaf9e158", "JX5456", "教学资料", "08db9271-efed-48e6-8f86-6604089a8b6e", "课程对比", "课程对比=>教学资料", 4),
        CreateDictionary("08db9272-8141-4584-82d4-f0a14dcba4a4", "KJ8174", "课件讲义", "08db9272-05ea-4d4e-8466-b7ceaaf9e158", "教学资料", "课程对比=>教学资料=>课件讲义", 1),
        CreateDictionary("08db9272-83b4-462f-8fa7-9b9e1614b90c", "KS7049", "考试大纲", "08db9272-05ea-4d4e-8466-b7ceaaf9e158", "教学资料", "课程对比=>教学资料=>考试大纲", 2),
        CreateDictionary("08db9272-85fb-4156-82fe-69186401b3d1", "JT07377", "其他", "08db9272-05ea-4d4e-8466-b7ceaaf9e158", "教学资料", "课程对比=>教学资料=>其他", 3),
        CreateDictionary("08db9272-0867-41f5-89f5-9a57f4f49012", "TS9189", "特色服务", "08db9271-efed-48e6-8f86-6604089a8b6e", "课程对比", "课程对比=>特色服务", 5),
        CreateDictionary("08db9272-8abf-4099-8400-b2dcac2be4ca", "XD2732", "续读协议", "08db9272-0867-41f5-89f5-9a57f4f49012", "特色服务", "课程对比=>特色服务=>续读协议", 1),
        CreateDictionary("08db9272-0aed-4ab8-81dc-ace77bea8494", "JG69532", "价格", "08db9271-efed-48e6-8f86-6604089a8b6e", "课程对比", "课程对比=>价格", 6),
    ];

    #endregion

    #region 续读原因

    private readonly Dictionaries[] _continueReason =
    [
        CreateDictionary("08db94bc-4eda-44e0-8b47-dd2e04233faf", "XD0266", "续读原因", null, "顶级节点", "续读原因"),
        CreateDictionary("08db94bc-6fdb-442b-8c29-4cad6d478fdd", "KS0862", "考试未通过", "08db94bc-4eda-44e0-8b47-dd2e04233faf", "续读原因", "续读原因=>考试未通过", 1),
        CreateDictionary("08db94bc-7408-4ef7-807e-c25a7ee3395e", "TS9519", "特殊原因缺考", "08db94bc-4eda-44e0-8b47-dd2e04233faf", "续读原因", "续读原因=>特殊原因缺考", 2),
        CreateDictionary("08db94bc-7732-4071-8723-a0c8d3f1173d", "BD0563", "本地区考试取消", "08db94bc-4eda-44e0-8b47-dd2e04233faf", "续读原因", "续读原因=>本地区考试取消", 3),
        CreateDictionary("08db94bc-7a93-4364-8757-e5e79055a7af", "JT8748", "其他原因", "08db94bc-4eda-44e0-8b47-dd2e04233faf", "续读原因", "续读原因=>其他原因", 4),
    ];

    #endregion

    #region 课程阶段

    private readonly Dictionaries[] _courseStage =
    [
        CreateDictionary("08db994f-e2e4-40d5-8ed4-18786bbf6e85", "KC9613", "课程阶段", null, "顶级节点", "课程阶段"),
        CreateDictionary("08db994f-eaed-458a-81c0-a61cc0a094cb", "JC1502", "基础阶段", "08db994f-e2e4-40d5-8ed4-18786bbf6e85", "课程阶段", "课程阶段=>基础阶段", 1),
        CreateDictionary("08db994f-eef1-4e74-8f38-828eccdb603e", "JH1476", "强化阶段", "08db994f-e2e4-40d5-8ed4-18786bbf6e85", "课程阶段", "课程阶段=>强化阶段", 2),
        CreateDictionary("08db994f-f5cb-47fa-861a-80a53511aba8", "CC5084", "冲刺阶段", "08db994f-e2e4-40d5-8ed4-18786bbf6e85", "课程阶段", "课程阶段=>冲刺阶段", 3),
        CreateDictionary("08db994f-fecf-42b8-87d5-41d7a0da7ac7", "PS6299", "评审阶段", "08db994f-e2e4-40d5-8ed4-18786bbf6e85", "课程阶段", "课程阶段=>评审阶段", 4),
        CreateDictionary("08db9950-037a-4b9d-86c4-472c075235d9", "JT29225", "其它", "08db994f-e2e4-40d5-8ed4-18786bbf6e85", "课程阶段", "课程阶段=>其它", 5),
    ];

    #endregion

    #region 热点类型

    private readonly Dictionaries[] _hotSpotType =
    [
        CreateDictionary("08dbceda-3f6b-4ae2-8493-9466c54511df", "RD0740", "热点类型", null, "顶级节点", "热点类型"),
        CreateDictionary("08dbceda-6e0d-4fd5-8eb3-2d281483f10f", "KS8313", "考试大纲", "08dbceda-3f6b-4ae2-8493-9466c54511df", "热点类型", "热点类型=>考试大纲", 1),
        CreateDictionary("08dbceda-7138-4013-8d26-18f6f3a1362b", "TS4849", "图书发售", "08dbceda-3f6b-4ae2-8493-9466c54511df", "热点类型", "热点类型=>图书发售", 2),
        CreateDictionary("08dbceda-73a5-4dae-822d-b43f65f9433d", "KS3233", "考试报名", "08dbceda-3f6b-4ae2-8493-9466c54511df", "热点类型", "热点类型=>考试报名", 3),
        CreateDictionary("08dbceda-75ba-423c-82eb-8ad5c2ed1630", "XX1600", "学习备考", "08dbceda-3f6b-4ae2-8493-9466c54511df", "热点类型", "热点类型=>学习备考", 4),
        CreateDictionary("08dbceda-782f-4fdd-8258-2533aec6727d", "ZK7113", "准考证", "08dbceda-3f6b-4ae2-8493-9466c54511df", "热点类型", "热点类型=>准考证", 5),
        CreateDictionary("08dbceda-a8b7-49a4-8ffa-efb7dcfe7164", "KS9201", "考试时间", "08dbceda-3f6b-4ae2-8493-9466c54511df", "热点类型", "热点类型=>考试时间", 6),
        CreateDictionary("08dbceda-ac1d-4976-8055-ce8a2afdad7b", "CJ4608", "成绩查询", "08dbceda-3f6b-4ae2-8493-9466c54511df", "热点类型", "热点类型=>成绩查询", 7),
        CreateDictionary("08dbceda-aefb-4a07-82f9-f2796027cc06", "ZS0482", "证书领取", "08dbceda-3f6b-4ae2-8493-9466c54511df", "热点类型", "热点类型=>证书领取", 8),
    ];

    #endregion

    #region 文章栏目

    private readonly Dictionaries[] _articleColumn =
    [
        CreateDictionary("08dbd42f-c05d-439f-8674-281eb54e8569", "WZ0151", "文章栏目", null, "顶级节点", "文章栏目"),
        CreateDictionary("08dbd42f-cdd2-4c93-8c91-78beba76b644", "KS3069", "考试热点", "08dbd42f-c05d-439f-8674-281eb54e8569", "文章栏目", "文章栏目=>考试热点", 1),
        CreateDictionary("08dbd42f-d0a9-4f0d-8b81-33445afe671e", "KS8129", "考试动态", "08dbd42f-c05d-439f-8674-281eb54e8569", "文章栏目", "文章栏目=>考试动态", 2),
    ];

    #endregion

    #region 能力维度

    private readonly Dictionaries[] _abilityDimension =
    [
        CreateDictionary("08dbcee8-a725-4dec-87b1-13d5be8c4cdd", "NL4892", "能力维度", null, "顶级节点", "能力维度"),
        CreateDictionary("d67222e1-539c-405a-bba7-442e0b517f5a", "JS2336", "计算能力", "08dbcee8-a725-4dec-87b1-13d5be8c4cdd", "能力维度", "能力维度=>计算能力", 1),
        CreateDictionary("08dbcee9-2047-4f57-809f-2129bedadb2c", "LX7165", "练习速度", "08dbcee8-a725-4dec-87b1-13d5be8c4cdd", "能力维度", "能力维度=>练习速度", 2),
        CreateDictionary("d67222e1-539c-405a-bba7-442e0b517f5d", "PD9144", "判断能力", "08dbcee8-a725-4dec-87b1-13d5be8c4cdd", "能力维度", "能力维度=>判断能力", 3),
        CreateDictionary("d67222e1-539c-405a-bba7-442e0b517f5f", "FX6064", "分析能力", "08dbcee8-a725-4dec-87b1-13d5be8c4cdd", "能力维度", "能力维度=>分析能力", 4),
        CreateDictionary("d67222e1-539c-405a-bba7-442e0b517f5c", "LJ6732", "理解能力", "08dbcee8-a725-4dec-87b1-13d5be8c4cdd", "能力维度", "能力维度=>理解能力", 5),
        CreateDictionary("d67222e1-539c-405a-bba7-442e0b517f5b", "JY4523", "记忆能力", "08dbcee8-a725-4dec-87b1-13d5be8c4cdd", "能力维度", "能力维度=>记忆能力", 6),
    ];

    #endregion

    #region 快递省份

    private readonly Dictionaries[] _expressProvince =
    [
        CreateDictionary("08dc04f7-1abf-4547-818f-acaac9f63747", "ProvinceCity", "省份城市", null, "顶级节点", "省份城市"),
        CreateDictionary("680dcbd7-bb10-4576-b963-d76b6e41fefe", "ExpressCompany", "快递公司", null, "顶级节点", "快递公司"),
    ];

    #endregion

    #region 组卷标签

    private readonly Dictionaries[] _paperTag =
    [
        CreateDictionary("06878503-c5b5-7e20-8000-bef46d57932e", "ZJ8239", "组卷标签", null, "顶级节点", "组卷标签"),
    ];

    #endregion

    private static Dictionaries CreateDictionary(string id, string code, string name, string pId, string pName, string path, int sort = 0)
    {
        return new Dictionaries
        {
            Id = new(id),
            Code = code,
            Name = name,
            Pid = pId == null ? null : new(pId),
            PName = pName,
            Path = path,
            Sort = sort,
            State = true,
            TenantId = Guid.Empty,
        };
    }
}