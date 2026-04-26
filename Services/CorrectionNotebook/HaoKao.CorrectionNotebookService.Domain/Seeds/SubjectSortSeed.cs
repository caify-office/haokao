using HaoKao.CorrectionNotebookService.Domain.Entities;

namespace HaoKao.CorrectionNotebookService.Domain.Seeds;

public class SubjectSortSeed
{
    private static SubjectSort Create(Guid id, Guid subjectId, int sort) => new()
    {
        Id = id,
        SubjectId = subjectId,
        CreatorId = Guid.Empty,
        Priority = sort,
        IsBuiltIn = true,
    };

    public static IReadOnlyList<SubjectSort> Data => [

        // 初级经济师 0190c3e7-a245-7983-997a-1141082743be
        Create(new("0190ca74-1c44-7b50-8af2-facbba282958"), new("0190c3ef-8743-71ce-968a-bb9140f63b3a"), 1),
        Create(new("0190ca74-1c44-7065-8ceb-4391642e4644"), new("0190c3ef-8743-7bf8-abb8-737d392178ea"), 2),
        Create(new("0190ca74-1c44-7573-a642-80193a552399"), new("0190c3ef-8743-762e-8650-f21b638463b4"), 3),
        Create(new("0190ca74-1c44-743b-b266-647862085453"), new("0190c3ef-8743-7170-8305-8f0c4ec6710f"), 4),
        Create(new("0190ca74-1c44-7b47-91b2-14cee948c838"), new("0190c3ef-8743-7316-80b9-3943ab0f4964"), 5),
        Create(new("0190ca74-1c44-7808-95ab-2c84298ce396"), new("0190c3ef-8743-7033-832f-f3e23ef3be39"), 6),
        Create(new("0190ca74-1c44-783f-9ccf-4f2586e3bc58"), new("0190c3ef-8743-7d39-9a41-6a8f593fbaf8"), 7),
        Create(new("0190ca74-1c44-782a-a835-a3c3073874c0"), new("0190c3ef-8743-79bf-8e03-4716c24dacd5"), 8),
        Create(new("0190ca74-1c44-7d13-bc87-331c5d8703f0"), new("0190c3ef-8743-7d1e-a683-0e4d8d0c2afa"), 9),
        Create(new("0190ca74-1c44-72bf-8a4f-1f87045b5d04"), new("0190c3ef-8743-7598-899a-2f0d0283f097"), 10),
        Create(new("0190ca74-1c44-79cf-92cc-6eb720470724"), new("0190c3ef-8743-7b68-a903-3b525bfd3493"), 11),

        // 中级经济师 0190c3e7-a245-7c1b-8630-a47ee4e91c09
        Create(new("0190ca74-1c44-7d10-8d81-40defafa7d3b"), new("0190c3ef-8743-7529-b37b-54200cf96680"), 1),
        Create(new("0190ca74-1c44-7109-be8c-5194444d7617"), new("0190c3ef-8743-76d3-be78-dbbbbc64b9da"), 2),
        Create(new("0190ca74-1c44-7e6d-98c7-f8241d96ac44"), new("0190c3ef-8743-7656-a6d8-eb98549cedb5"), 3),
        Create(new("0190ca74-1c44-7928-a2c0-1e454f1b3426"), new("0190c3ef-8743-72b6-b702-b47b64eb8a04"), 4),
        Create(new("0190ca74-1c44-71b7-b1ac-916c5244bc76"), new("0190c3ef-8743-76a6-8ce7-426d5e5e723f"), 5),
        Create(new("0190ca74-1c44-72d0-b2af-1b12098ffb06"), new("0190c3ef-8743-7896-bbc5-bfa6493ccc9a"), 6),
        Create(new("0190ca74-1c44-7301-ada3-fcefc48bfcd4"), new("0190c3ef-8743-7071-bb77-c3c115726963"), 7),
        Create(new("0190ca74-1c44-78c8-b51d-68ec0bbf8336"), new("0190c3ef-8743-72ff-bbb7-1e179ec221ba"), 8),
        Create(new("0190ca74-1c44-7827-a73a-33bdcff42b25"), new("0190c3ef-8743-7713-a03d-061ba4f41c8a"), 9),
        Create(new("0190ca74-1c44-73bc-b9b9-cd4f459cb77e"), new("0190c3ef-8743-7ec9-a5e4-0b49f9d39509"), 10),
        Create(new("0190ca74-1c44-73a7-af2d-9d42b8c79951"), new("0190c3ef-8743-7dbc-b823-906196e0f02a"), 11),

        // 高级经济师 0190c3e7-a245-7fe3-ac55-f79166ebc5c9
        Create(new("0190ca74-1c44-7ebd-9f42-4278026948cd"), new("0190c3ef-8743-7b4c-aa7f-5ffe2e771de3"), 1),
        Create(new("0190ca74-1c44-7005-b5fd-17475dc6fa10"), new("0190c3ef-8743-792c-aa5b-cdb702201ebf"), 2),
        Create(new("0190ca74-1c44-7c98-b5a9-8aa9aa3eba15"), new("0190c3ef-8743-7db1-b3d5-f308bedd33bb"), 3),
        Create(new("0190ca74-1c44-7e83-b2b1-ca7d6164768a"), new("0190c3ef-8743-7dd6-89a9-946d858a5700"), 4),
        Create(new("0190ca74-1c44-71f8-8ca5-8edf01cadf27"), new("0190c3ef-8744-7566-9fd5-8c07537131da"), 5),
        Create(new("0190ca74-1c44-79c9-83fd-b778c6aca4c2"), new("0190c3ef-8744-7f81-bce2-4ae76d963b81"), 6),
        Create(new("0190ca74-1c44-776b-8c42-a3dcf2887f3f"), new("0190c3ef-8744-7992-aa0c-e2e8efb7be8b"), 7),
        Create(new("0190ca74-1c44-7b87-b099-5c44eb3f26f3"), new("0190c3ef-8744-7093-aacf-5178a34306f3"), 8),
        Create(new("0190ca74-1c44-7551-9dc1-94709d479d7b"), new("0190c3ef-8744-7da6-9939-cce429b59d47"), 9),
        Create(new("0190ca74-1c45-7357-8866-5d4ccc6ec2a6"), new("0190c3ef-8744-7be0-8665-3cf3e4025b1c"), 10),

        // 注册会计师 0190c3e7-a245-7d10-9754-339536267635
        Create(new("0190ca74-1c45-7427-a87f-8cceb68e1bad"), new("0190c3ef-8744-7953-9b9e-036d64f9d722"), 1),
        Create(new("0190ca74-1c45-72ad-bda2-cef3016b8c46"), new("0190c3ef-8744-777a-b985-0adeeb41e0cf"), 2),
        Create(new("0190ca74-1c45-72f6-bda1-a3be237469b0"), new("0190c3ef-8744-7e4f-a046-352818453d75"), 3),
        Create(new("0190ca74-1c45-7ff1-8efe-b87c2f7b72f0"), new("0190c3ef-8744-7fde-9122-582cd15a8fda"), 4),
        Create(new("0190ca74-1c45-715e-a5a6-dae02cf42571"), new("0190c3ef-8744-799c-a00f-1764d9c593cc"), 5),
        Create(new("0190ca74-1c45-7853-8682-a99091882d94"), new("0190c3ef-8744-7539-8d73-27d88bdda825"), 6),

        // 中级会计职称 0190c3e7-a245-7efd-8d62-8a061fe71b22
        Create(new("0190ca74-1c45-711e-904d-e3e3cd98a053"), new("0190c3ef-8744-7c45-ab86-a8cede07a368"), 1),
        Create(new("0190ca74-1c45-78f8-b4e9-c23e59c00b38"), new("0190c3ef-8744-7adb-b2f6-165be84e2549"), 2),
        Create(new("0190ca74-1c45-712d-a6b1-2bd194e1ea01"), new("0190c3ef-8744-79bf-869c-914549f73938"), 3),

        // 初级社会工作者 0190c3e7-a245-7cb5-9613-122673d29071
        Create(new("0190ca74-1c45-7e08-b3ca-b9ca1d016483"), new("0190c3ef-8744-721c-9f39-fdd3b2581276"), 1),
        Create(new("0190ca74-1c45-7386-bf89-3165cad1cfaa"), new("0190c3ef-8744-76b3-b54f-79dff76044b4"), 2),

        // 中级社会工作者 0190c3e7-a245-7b3c-b30f-4626b1f94b76
        Create(new("0190ca74-1c45-78e7-a0c3-72ed0756fdc5"), new("0190c3ef-8744-7f14-8aec-9a55efe68c2f"), 1),
        Create(new("0190ca74-1c45-79a9-b73f-630e96eb52f9"), new("0190c3ef-8744-7805-8748-b1a7d4bb554b"), 2),
        Create(new("0190ca74-1c45-7774-a130-c28f2775fea8"), new("0190c3ef-8744-74f3-b994-f6e9c30b86a7"), 3),

        // 软考中级职称 0190c3e7-a245-71ce-ba7d-6c26e155cc17
        Create(new("0190ca74-1c45-77cc-96d9-e1525db6eeba"), new("0190c3ef-8744-7bbe-a6e7-d3eabac9e5c3"), 1),
        Create(new("0190ca74-1c45-736c-8026-4bc5d657194f"), new("0190c3ef-8744-75e7-b691-7db6e6f67170"), 2),
        Create(new("0190ca74-1c45-7b3d-af06-7aab27a0fbcc"), new("0190c3ef-8744-7930-86f6-6afbf0b1e5e9"), 3),
        Create(new("0190ca74-1c45-7346-a26b-70123f5e67e2"), new("0190c3ef-8744-7164-8c80-bbd332272481"), 4),
        Create(new("0190ca74-1c45-7a8c-9466-ac1c7dde0c82"), new("0190c3ef-8744-73d8-963f-8f0535f7f43c"), 5),
        Create(new("0190ca74-1c45-70aa-8421-4ece42785e76"), new("0190c3ef-8744-7e71-a99b-1b31d3ccbf0c"), 6),
        Create(new("0190ca74-1c45-74f4-8436-871e3eb22072"), new("0190c3ef-8744-7263-a310-8aa4054d75e2"), 7),
        Create(new("0190ca74-1c45-7e1f-aae8-41718b5a67fa"), new("0190c3ef-8744-7f15-be59-7c9aa57db72d"), 8),
        Create(new("0190ca74-1c45-71fb-a47e-243da0cf1940"), new("0190c3ef-8744-732a-81a4-b368da5f182c"), 9),

        // 软考高级职称 0190c3e7-a245-7f93-ad52-3bc70bd9dcce
        Create(new("0190ca74-1c45-788c-ae35-ba0197e0e35c"), new("0190c3ef-8744-79f6-98d1-fa17543852f0"), 1),
        Create(new("0190ca74-1c45-773a-a605-d073e7f4f0a2"), new("0190c3ef-8744-73e5-94cd-8fcacb2d3f54"), 2),
        Create(new("0190ca74-1c45-7fd9-a220-c768f0ba99d1"), new("0190c3ef-8744-7c93-870a-bd345d0d9850"), 3),
        Create(new("0190ca74-1c45-7f04-ae8a-5d16bb2b85b8"), new("0190c3ef-8745-747f-8e86-91784dcde76f"), 4),
        Create(new("0190ca74-1c45-7e00-b18f-7aefbcbe2f2a"), new("0190c3ef-8745-783a-b8ed-f68787eeff35"), 5),
    ];
}