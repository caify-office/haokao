using HaoKao.CorrectionNotebookService.Domain.Entities;

namespace HaoKao.CorrectionNotebookService.Domain.Seeds;

public class SubjectSeed
{
    private static Subject Create(Guid id, Guid examLevelId, string name, string icon) => new()
    {
        Id = id,
        ExamLevelId = examLevelId,
        Name = name,
        IsBuiltIn = true,
        CreatorId = Guid.Empty,
        CreateTime = new DateTime(2024, 1, 1),
        Icon = icon, 
    };

    public static IReadOnlyList<Subject> Data => [

        // 初级经济师 0190c3e7-a245-7983-997a-1141082743be
        Create(new("0190c3ef-8743-71ce-968a-bb9140f63b3a"), new("0190c3e7-a245-7983-997a-1141082743be"), "经济基础", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E7%BB%8F%E6%B5%8E%E5%9F%BA%E7%A1%80%402x.png"),
        Create(new("0190c3ef-8743-7bf8-abb8-737d392178ea"), new("0190c3e7-a245-7983-997a-1141082743be"), "人力资源管理", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E4%BA%BA%E5%8A%9B%E8%B5%84%E6%BA%90%E7%AE%A1%E7%90%86%402x.png"),
        Create(new("0190c3ef-8743-762e-8650-f21b638463b4"), new("0190c3e7-a245-7983-997a-1141082743be"), "工商管理", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%B7%A5%E5%95%86%E7%AE%A1%E7%90%86%402x.png"),
        Create(new("0190c3ef-8743-7170-8305-8f0c4ec6710f"), new("0190c3e7-a245-7983-997a-1141082743be"), "金融", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E9%87%91%E8%9E%8D%402x.png"),
        Create(new("0190c3ef-8743-7316-80b9-3943ab0f4964"), new("0190c3e7-a245-7983-997a-1141082743be"), "建筑与房地产经济", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%BB%BA%E7%AD%91%E4%B8%8E%E6%88%BF%E5%9C%B0%E4%BA%A7%402x.png"),
        Create(new("0190c3ef-8743-7033-832f-f3e23ef3be39"), new("0190c3e7-a245-7983-997a-1141082743be"), "知识产权", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E7%9F%A5%E8%AF%86%E4%BA%A7%E6%9D%83%402x.png"),
        Create(new("0190c3ef-8743-7d39-9a41-6a8f593fbaf8"), new("0190c3e7-a245-7983-997a-1141082743be"), "财政税收", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E8%B4%A2%E6%94%BF%E7%A8%8E%E6%94%B6%402x.png"),
        Create(new("0190c3ef-8743-79bf-8e03-4716c24dacd5"), new("0190c3e7-a245-7983-997a-1141082743be"), "运输经济", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E8%BF%90%E8%BE%93%E7%BB%8F%E6%B5%8E%402x.png"),
        Create(new("0190c3ef-8743-7d1e-a683-0e4d8d0c2afa"), new("0190c3e7-a245-7983-997a-1141082743be"), "农业经济", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%86%9C%E4%B8%9A%E7%BB%8F%E6%B5%8E%402x.png"),
        Create(new("0190c3ef-8743-7598-899a-2f0d0283f097"), new("0190c3e7-a245-7983-997a-1141082743be"), "旅游经济", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E6%97%85%E6%B8%B8%402x.png"),
        Create(new("0190c3ef-8743-7b68-a903-3b525bfd3493"), new("0190c3e7-a245-7983-997a-1141082743be"), "保险", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E4%BF%9D%E9%99%A9%402x.png"),

        // 中级经济师 0190c3e7-a245-7c1b-8630-a47ee4e91c09
        Create(new("0190c3ef-8743-7529-b37b-54200cf96680"), new("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "经济基础", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E7%BB%8F%E6%B5%8E%E5%9F%BA%E7%A1%80%402x.png"),
        Create(new("0190c3ef-8743-76d3-be78-dbbbbc64b9da"), new("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "人力资源管理", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E4%BA%BA%E5%8A%9B%E8%B5%84%E6%BA%90%E7%AE%A1%E7%90%86%402x.png"),
        Create(new("0190c3ef-8743-7656-a6d8-eb98549cedb5"), new("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "工商管理", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%B7%A5%E5%95%86%E7%AE%A1%E7%90%86%402x.png"),
        Create(new("0190c3ef-8743-72b6-b702-b47b64eb8a04"), new("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "金融", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E9%87%91%E8%9E%8D%402x.png"),
        Create(new("0190c3ef-8743-76a6-8ce7-426d5e5e723f"), new("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "建筑与房地产经济", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%BB%BA%E7%AD%91%E4%B8%8E%E6%88%BF%E5%9C%B0%E4%BA%A7%402x.png"),
        Create(new("0190c3ef-8743-7896-bbc5-bfa6493ccc9a"), new("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "知识产权", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E7%9F%A5%E8%AF%86%E4%BA%A7%E6%9D%83%402x.png"),
        Create(new("0190c3ef-8743-7071-bb77-c3c115726963"), new("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "财政税收", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E8%B4%A2%E6%94%BF%E7%A8%8E%E6%94%B6%402x.png"),
        Create(new("0190c3ef-8743-72ff-bbb7-1e179ec221ba"), new("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "运输经济", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E8%BF%90%E8%BE%93%E7%BB%8F%E6%B5%8E%402x.png"),
        Create(new("0190c3ef-8743-7713-a03d-061ba4f41c8a"), new("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "农业经济", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%86%9C%E4%B8%9A%E7%BB%8F%E6%B5%8E%402x.png"),
        Create(new("0190c3ef-8743-7ec9-a5e4-0b49f9d39509"), new("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "旅游经济", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E6%97%85%E6%B8%B8%402x.png"),
        Create(new("0190c3ef-8743-7dbc-b823-906196e0f02a"), new("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "保险", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E4%BF%9D%E9%99%A9%402x.png"),

        // 高级经济师 0190c3e7-a245-7fe3-ac55-f79166ebc5c9
        Create(new("0190c3ef-8743-7b4c-aa7f-5ffe2e771de3"), new("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "人力资源管理", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E4%BA%BA%E5%8A%9B%E8%B5%84%E6%BA%90%E7%AE%A1%E7%90%86%402x.png"),
        Create(new("0190c3ef-8743-792c-aa5b-cdb702201ebf"), new("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "工商管理", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%B7%A5%E5%95%86%E7%AE%A1%E7%90%86%402x.png"),
        Create(new("0190c3ef-8743-7db1-b3d5-f308bedd33bb"), new("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "金融", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E9%87%91%E8%9E%8D%402x.png"),
        Create(new("0190c3ef-8743-7dd6-89a9-946d858a5700"), new("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "建筑与房地产经济", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%BB%BA%E7%AD%91%E4%B8%8E%E6%88%BF%E5%9C%B0%E4%BA%A7%402x.png"),
        Create(new("0190c3ef-8744-7566-9fd5-8c07537131da"), new("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "知识产权", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E7%9F%A5%E8%AF%86%E4%BA%A7%E6%9D%83%402x.png"),
        Create(new("0190c3ef-8744-7f81-bce2-4ae76d963b81"), new("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "财政税收", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E8%B4%A2%E6%94%BF%E7%A8%8E%E6%94%B6%402x.png"),
        Create(new("0190c3ef-8744-7992-aa0c-e2e8efb7be8b"), new("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "运输经济", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E8%BF%90%E8%BE%93%E7%BB%8F%E6%B5%8E%402x.png"),
        Create(new("0190c3ef-8744-7093-aacf-5178a34306f3"), new("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "农业经济", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%86%9C%E4%B8%9A%E7%BB%8F%E6%B5%8E%402x.png"),
        Create(new("0190c3ef-8744-7da6-9939-cce429b59d47"), new("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "旅游经济", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E6%97%85%E6%B8%B8%402x.png"),
        Create(new("0190c3ef-8744-7be0-8665-3cf3e4025b1c"), new("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "保险", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E4%BF%9D%E9%99%A9%402x.png"),

        // 注册会计师 0190c3e7-a245-7d10-9754-339536267635
        Create(new("0190c3ef-8744-7953-9b9e-036d64f9d722"), new("0190c3e7-a245-7d10-9754-339536267635"), "公司战略与风险管理", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E5%85%AC%E5%8F%B8%E6%88%98%E7%95%A5%E4%B8%8E%E9%A3%8E%E9%99%A9%E7%AE%A1%E7%90%86%402x.png"),
        Create(new("0190c3ef-8744-777a-b985-0adeeb41e0cf"), new("0190c3e7-a245-7d10-9754-339536267635"), "经济法", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E7%BB%8F%E6%B5%8E%E6%B3%95%402x.png"),
        Create(new("0190c3ef-8744-7e4f-a046-352818453d75"), new("0190c3e7-a245-7d10-9754-339536267635"), "会计", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E4%BC%9A%E8%AE%A1%402x.png"),
        Create(new("0190c3ef-8744-7fde-9122-582cd15a8fda"), new("0190c3e7-a245-7d10-9754-339536267635"), "税法", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E7%A8%8E%E6%B3%95%402x.png"),
        Create(new("0190c3ef-8744-799c-a00f-1764d9c593cc"), new("0190c3e7-a245-7d10-9754-339536267635"), "财务成本管理", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E8%B4%A2%E5%8A%A1%E6%88%90%E6%9C%AC%E7%AE%A1%E7%90%86%402x.png"),
        Create(new("0190c3ef-8744-7539-8d73-27d88bdda825"), new("0190c3e7-a245-7d10-9754-339536267635"), "审计", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E5%AE%A1%E8%AE%A1%402x.png"),

        // 中级会计职称 0190c3e7-a245-7efd-8d62-8a061fe71b22
        Create(new("0190c3ef-8744-7c45-ab86-a8cede07a368"), new("0190c3e7-a245-7efd-8d62-8a061fe71b22"), "经济法", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E7%BB%8F%E6%B5%8E%E6%B3%95%E5%9F%BA%E7%A1%80%402x.png"),
        Create(new("0190c3ef-8744-7adb-b2f6-165be84e2549"), new("0190c3e7-a245-7efd-8d62-8a061fe71b22"), "财务管理", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E8%B4%A2%E5%8A%A1%E7%AE%A1%E7%90%86%402x.png"),
        Create(new("0190c3ef-8744-79bf-869c-914549f73938"), new("0190c3e7-a245-7efd-8d62-8a061fe71b22"), "中级会计实务", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E4%BC%9A%E8%AE%A1%E5%AE%9E%E5%8A%A1%402x.png"),

        // 初级社会工作者 0190c3e7-a245-7cb5-9613-122673d29071
        Create(new("0190c3ef-8744-721c-9f39-fdd3b2581276"), new("0190c3e7-a245-7cb5-9613-122673d29071"), "社会工作实务", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/sg/%E7%A4%BE%E4%BC%9A%E5%B7%A5%E4%BD%9C%E5%AE%9E%E5%8A%A1%402x.png"),
        Create(new("0190c3ef-8744-76b3-b54f-79dff76044b4"), new("0190c3e7-a245-7cb5-9613-122673d29071"), "社会工作综合能力", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/sg/%E7%A4%BE%E4%BC%9A%E5%B7%A5%E4%BD%9C%E7%BB%BC%E5%90%88%E8%83%BD%E5%8A%9B%402x.png"),

        // 中级社会工作者 0190c3e7-a245-7b3c-b30f-4626b1f94b76
        Create(new("0190c3ef-8744-7f14-8aec-9a55efe68c2f"), new("0190c3e7-a245-7b3c-b30f-4626b1f94b76"), "社会工作实务", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/sg/%E7%A4%BE%E4%BC%9A%E5%B7%A5%E4%BD%9C%E5%AE%9E%E5%8A%A1%402x.png"),
        Create(new("0190c3ef-8744-7805-8748-b1a7d4bb554b"), new("0190c3e7-a245-7b3c-b30f-4626b1f94b76"), "社会工作法规与政策", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/sg/%E7%A4%BE%E4%BC%9A%E5%B7%A5%E4%BD%9C%E6%B3%95%E8%A7%84%E4%B8%8E%E6%94%BF%E7%AD%96%402x.png"),
        Create(new("0190c3ef-8744-74f3-b994-f6e9c30b86a7"), new("0190c3e7-a245-7b3c-b30f-4626b1f94b76"), "社会工作综合能力", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/sg/%E7%A4%BE%E4%BC%9A%E5%B7%A5%E4%BD%9C%E7%BB%BC%E5%90%88%E8%83%BD%E5%8A%9B%402x.png"),

        // 软考中级职称 0190c3e7-a245-71ce-ba7d-6c26e155cc17
        Create(new("0190c3ef-8744-7bbe-a6e7-d3eabac9e5c3"), new("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "系统集成项目管理工程师", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E7%B3%BB%E7%BB%9F%E9%9B%86%E6%88%90%E9%A1%B9%E7%9B%AE%E7%AE%A1%E7%90%86%E5%B7%A5%E7%A8%8B%E5%B8%88%402x.png"),
        Create(new("0190c3ef-8744-75e7-b691-7db6e6f67170"), new("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "软件设计师", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E8%BD%AF%E4%BB%B6%E8%AE%BE%E8%AE%A1%E5%B8%88%402x.png"),
        Create(new("0190c3ef-8744-7930-86f6-6afbf0b1e5e9"), new("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "数据库系统工程师", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E6%95%B0%E6%8D%AE%E5%BA%93%E7%B3%BB%E7%BB%9F%E5%B7%A5%E7%A8%8B%E5%B8%88%402x.png"),
        Create(new("0190c3ef-8744-7164-8c80-bbd332272481"), new("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "电子商务设计师", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E7%94%B5%E5%AD%90%E5%95%86%E5%8A%A1%E8%AE%BE%E8%AE%A1%E5%B8%88%402x.png"),
        Create(new("0190c3ef-8744-73d8-963f-8f0535f7f43c"), new("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "网络工程师", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E7%BD%91%E7%BB%9C%E5%B7%A5%E7%A8%8B%E5%B8%88%402x.png"),
        Create(new("0190c3ef-8744-7e71-a99b-1b31d3ccbf0c"), new("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "信息安全工程师", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E4%BF%A1%E6%81%AF%E5%AE%89%E5%85%A8%E5%B7%A5%E7%A8%8B%E5%B8%88%402x.png"),
        Create(new("0190c3ef-8744-7263-a310-8aa4054d75e2"), new("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "信息系统监理师", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E4%BF%A1%E6%81%AF%E7%B3%BB%E7%BB%9F%E7%9B%91%E7%90%86%E5%B8%88%402x.png"),
        Create(new("0190c3ef-8744-7f15-be59-7c9aa57db72d"), new("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "信息系统管理工程师", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E4%BF%A1%E6%81%AF%E7%B3%BB%E7%BB%9F%E7%AE%A1%E7%90%86%E5%B7%A5%E7%A8%8B%E5%B8%88%402x.png"),
        Create(new("0190c3ef-8744-732a-81a4-b368da5f182c"), new("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "软件评测师", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E8%BD%AF%E4%BB%B6%E8%AF%84%E6%B5%8B%E5%B8%88%402x.png"),

        // 软考高级职称 0190c3e7-a245-7f93-ad52-3bc70bd9dcce
        Create(new("0190c3ef-8744-79f6-98d1-fa17543852f0"), new("0190c3e7-a245-7f93-ad52-3bc70bd9dcce"), "信息系统项目管理师", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E4%BF%A1%E6%81%AF%E7%B3%BB%E7%BB%9F%E9%A1%B9%E7%9B%AE%E7%AE%A1%E7%90%86%E5%B8%88%402x.png"),
        Create(new("0190c3ef-8744-73e5-94cd-8fcacb2d3f54"), new("0190c3e7-a245-7f93-ad52-3bc70bd9dcce"), "系统分析师", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E7%B3%BB%E7%BB%9F%E5%88%86%E6%9E%90%E5%B8%88%402x.png"),
        Create(new("0190c3ef-8744-7c93-870a-bd345d0d9850"), new("0190c3e7-a245-7f93-ad52-3bc70bd9dcce"), "网络规划设计师", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E7%BD%91%E7%BB%9C%E8%A7%84%E8%AE%BE%E8%AE%A1%E5%B8%88%402x.png"),
        Create(new("0190c3ef-8745-747f-8e86-91784dcde76f"), new("0190c3e7-a245-7f93-ad52-3bc70bd9dcce"), "系统规划与管理师", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E7%B3%BB%E7%BB%9F%E8%A7%84%E5%88%92%E4%B8%8E%E7%AE%A1%E7%90%86%E5%B8%88%402x.png"),
        Create(new("0190c3ef-8745-783a-b8ed-f68787eeff35"), new("0190c3e7-a245-7f93-ad52-3bc70bd9dcce"), "系统架构师", "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E7%B3%BB%E7%BB%9F%E6%9E%B6%E6%9E%84%E8%AE%BE%E8%AE%A1%E5%B8%88%402x.png"),
    ];
}