using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HaoKao.CorrectionNotebookService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExamLevel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "考试级别名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsBuiltIn = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否内置数据"),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "创建人Id", collation: "ascii_general_ci"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "父级Id", collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamLevel", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Category = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, comment: "标签分类")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, comment: "标签名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsBuiltIn = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否内置数据"),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "创建人Id", collation: "ascii_general_ci"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ExamLevelId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "考试级别Id", collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "科目名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "text", nullable: false, comment: "科目图标")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsBuiltIn = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否内置数据"),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "创建人Id", collation: "ascii_general_ci"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subject_ExamLevel_ExamLevelId",
                        column: x => x.ExamLevelId,
                        principalTable: "ExamLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ExamLevelId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "考试级别Id", collation: "ascii_general_ci"),
                    SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "科目Id", collation: "ascii_general_ci"),
                    ImageUrl = table.Column<string>(type: "text", nullable: false, comment: "题目图片地址")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "text", nullable: false, comment: "题目内容(文本)")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Answer = table.Column<string>(type: "text", nullable: false, comment: "题目答案")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Analysis = table.Column<string>(type: "text", nullable: false, comment: "题目解析")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GenerationTimes = table.Column<int>(type: "int", nullable: false, comment: "生成次数"),
                    Generatable = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否可生成答案和解析"),
                    MasteryDegree = table.Column<int>(type: "int", nullable: false, comment: "掌握程度"),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "创建人Id", collation: "ascii_general_ci"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SubjectSort",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "科目Id", collation: "ascii_general_ci"),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "创建人Id", collation: "ascii_general_ci"),
                    Priority = table.Column<int>(type: "int", nullable: false, comment: "排序"),
                    IsBuiltIn = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否内置数据")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectSort", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectSort_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GenerationLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    QuestionId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "题目Id", collation: "ascii_general_ci"),
                    Answer = table.Column<string>(type: "text", nullable: false, comment: "题目答案")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Analysis = table.Column<string>(type: "text", nullable: false, comment: "题目解析")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: false, comment: "创建者Id", collation: "ascii_general_ci"),
                    CreatorName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "创建者名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenerationLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GenerationLog_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QuestionTag",
                columns: table => new
                {
                    QuestionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TagId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTag", x => new { x.QuestionId, x.TagId });
                    table.ForeignKey(
                        name: "FK_QuestionTag_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "ExamLevel",
                columns: new[] { "Id", "CreateTime", "CreatorId", "IsBuiltIn", "Name", "ParentId" },
                values: new object[,]
                {
                    { new Guid("0190c3e6-3214-735b-8d36-7b41ccd9918a"), new DateTime(2024, 1, 1, 0, 0, 5, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "会计师", new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("0190c3e6-3214-776a-9f19-5bd81cf0b8c0"), new DateTime(2024, 1, 1, 0, 0, 11, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "计算机软件资格", new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("0190c3e6-3214-7b8b-b2ae-0d578805c176"), new DateTime(2024, 1, 1, 0, 0, 8, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "社会工作者", new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("0190c3e6-3214-7dc6-bc25-6b0a5cab3d9e"), new DateTime(2024, 1, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "经济师", new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), new DateTime(2024, 1, 1, 0, 0, 12, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "软考中级职称", new Guid("0190c3e6-3214-776a-9f19-5bd81cf0b8c0") },
                    { new Guid("0190c3e7-a245-7983-997a-1141082743be"), new DateTime(2024, 1, 1, 0, 0, 2, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "初级经济师", new Guid("0190c3e6-3214-7dc6-bc25-6b0a5cab3d9e") },
                    { new Guid("0190c3e7-a245-7b3c-b30f-4626b1f94b76"), new DateTime(2024, 1, 1, 0, 0, 10, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "中级社会工作者", new Guid("0190c3e6-3214-7b8b-b2ae-0d578805c176") },
                    { new Guid("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), new DateTime(2024, 1, 1, 0, 0, 3, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "中级经济师", new Guid("0190c3e6-3214-7dc6-bc25-6b0a5cab3d9e") },
                    { new Guid("0190c3e7-a245-7cb5-9613-122673d29071"), new DateTime(2024, 1, 1, 0, 0, 9, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "初级社会工作者", new Guid("0190c3e6-3214-7b8b-b2ae-0d578805c176") },
                    { new Guid("0190c3e7-a245-7d10-9754-339536267635"), new DateTime(2024, 1, 1, 0, 0, 6, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "注册会计师", new Guid("0190c3e6-3214-735b-8d36-7b41ccd9918a") },
                    { new Guid("0190c3e7-a245-7efd-8d62-8a061fe71b22"), new DateTime(2024, 1, 1, 0, 0, 7, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "中级会计职称", new Guid("0190c3e6-3214-735b-8d36-7b41ccd9918a") },
                    { new Guid("0190c3e7-a245-7f93-ad52-3bc70bd9dcce"), new DateTime(2024, 1, 1, 0, 0, 13, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "软考高级职称", new Guid("0190c3e6-3214-776a-9f19-5bd81cf0b8c0") },
                    { new Guid("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), new DateTime(2024, 1, 1, 0, 0, 4, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "高级经济师", new Guid("0190c3e6-3214-7dc6-bc25-6b0a5cab3d9e") }
                });

            migrationBuilder.InsertData(
                table: "Tag",
                columns: new[] { "Id", "Category", "CreateTime", "CreatorId", "IsBuiltIn", "Name" },
                values: new object[,]
                {
                    { new Guid("0190fd34-7e0c-70b6-a3c9-7ea275f314a7"), "难易程度", new DateTime(2024, 1, 1, 0, 0, 11, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "比较难" },
                    { new Guid("0190fd34-7e0c-70ed-b554-cab99cf71a2b"), "题型", new DateTime(2024, 1, 1, 0, 0, 1, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "单选题" },
                    { new Guid("0190fd34-7e0c-74cb-95fa-2204eb2217b1"), "题型", new DateTime(2024, 1, 1, 0, 0, 5, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "填空题" },
                    { new Guid("0190fd34-7e0c-76b0-ad52-3b531e8a7756"), "错误原因", new DateTime(2024, 1, 1, 0, 0, 17, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "其他" },
                    { new Guid("0190fd34-7e0c-76c4-95dc-b85db92445d8"), "题型", new DateTime(2024, 1, 1, 0, 0, 7, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "问答题" },
                    { new Guid("0190fd34-7e0c-76d4-92a6-5e2ee9b06a97"), "题型", new DateTime(2024, 1, 1, 0, 0, 6, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "案例分析题" },
                    { new Guid("0190fd34-7e0c-776d-8ecb-684b43b395c7"), "错误原因", new DateTime(2024, 1, 1, 0, 0, 15, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "没有思路" },
                    { new Guid("0190fd34-7e0c-77a1-ba5c-6ea1e2d0d06b"), "题型", new DateTime(2024, 1, 1, 0, 0, 8, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "其他" },
                    { new Guid("0190fd34-7e0c-7927-8b80-e5c337fba29e"), "难易程度", new DateTime(2024, 1, 1, 0, 0, 9, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "简单" },
                    { new Guid("0190fd34-7e0c-7a1c-9f34-6449080abdf3"), "难易程度", new DateTime(2024, 1, 1, 0, 0, 12, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "非常难" },
                    { new Guid("0190fd34-7e0c-7c03-af63-0482f1f8177b"), "题型", new DateTime(2024, 1, 1, 0, 0, 4, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "判断题" },
                    { new Guid("0190fd34-7e0c-7c19-ae1a-e069e46dad5a"), "题型", new DateTime(2024, 1, 1, 0, 0, 3, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "不定项选择题" },
                    { new Guid("0190fd34-7e0c-7d33-87eb-fc7fdee7df7c"), "错误原因", new DateTime(2024, 1, 1, 0, 0, 13, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "马虎粗心" },
                    { new Guid("0190fd34-7e0c-7d4d-aef5-a7e00f35c5bb"), "难易程度", new DateTime(2024, 1, 1, 0, 0, 10, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "一般" },
                    { new Guid("0190fd34-7e0c-7e34-8775-33f1dfa66fb7"), "错误原因", new DateTime(2024, 1, 1, 0, 0, 16, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "运算错误" },
                    { new Guid("0190fd34-7e0c-7e98-84f5-c0cfbf5e27d1"), "错误原因", new DateTime(2024, 1, 1, 0, 0, 14, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "概念未掌握" },
                    { new Guid("0190fd34-7e0c-7f7d-97a8-e6f31310ea90"), "题型", new DateTime(2024, 1, 1, 0, 0, 2, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), true, "多选题" }
                });

            migrationBuilder.InsertData(
                table: "Subject",
                columns: new[] { "Id", "CreateTime", "CreatorId", "ExamLevelId", "Icon", "IsBuiltIn", "Name" },
                values: new object[,]
                {
                    { new Guid("0190c3ef-8743-7033-832f-f3e23ef3be39"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7983-997a-1141082743be"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E7%9F%A5%E8%AF%86%E4%BA%A7%E6%9D%83%402x.png", true, "知识产权" },
                    { new Guid("0190c3ef-8743-7071-bb77-c3c115726963"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E8%B4%A2%E6%94%BF%E7%A8%8E%E6%94%B6%402x.png", true, "财政税收" },
                    { new Guid("0190c3ef-8743-7170-8305-8f0c4ec6710f"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7983-997a-1141082743be"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E9%87%91%E8%9E%8D%402x.png", true, "金融" },
                    { new Guid("0190c3ef-8743-71ce-968a-bb9140f63b3a"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7983-997a-1141082743be"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E7%BB%8F%E6%B5%8E%E5%9F%BA%E7%A1%80%402x.png", true, "经济基础" },
                    { new Guid("0190c3ef-8743-72b6-b702-b47b64eb8a04"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E9%87%91%E8%9E%8D%402x.png", true, "金融" },
                    { new Guid("0190c3ef-8743-72ff-bbb7-1e179ec221ba"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E8%BF%90%E8%BE%93%E7%BB%8F%E6%B5%8E%402x.png", true, "运输经济" },
                    { new Guid("0190c3ef-8743-7316-80b9-3943ab0f4964"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7983-997a-1141082743be"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%BB%BA%E7%AD%91%E4%B8%8E%E6%88%BF%E5%9C%B0%E4%BA%A7%402x.png", true, "建筑与房地产经济" },
                    { new Guid("0190c3ef-8743-7529-b37b-54200cf96680"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E7%BB%8F%E6%B5%8E%E5%9F%BA%E7%A1%80%402x.png", true, "经济基础" },
                    { new Guid("0190c3ef-8743-7598-899a-2f0d0283f097"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7983-997a-1141082743be"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E6%97%85%E6%B8%B8%402x.png", true, "旅游经济" },
                    { new Guid("0190c3ef-8743-762e-8650-f21b638463b4"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7983-997a-1141082743be"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%B7%A5%E5%95%86%E7%AE%A1%E7%90%86%402x.png", true, "工商管理" },
                    { new Guid("0190c3ef-8743-7656-a6d8-eb98549cedb5"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%B7%A5%E5%95%86%E7%AE%A1%E7%90%86%402x.png", true, "工商管理" },
                    { new Guid("0190c3ef-8743-76a6-8ce7-426d5e5e723f"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%BB%BA%E7%AD%91%E4%B8%8E%E6%88%BF%E5%9C%B0%E4%BA%A7%402x.png", true, "建筑与房地产经济" },
                    { new Guid("0190c3ef-8743-76d3-be78-dbbbbc64b9da"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E4%BA%BA%E5%8A%9B%E8%B5%84%E6%BA%90%E7%AE%A1%E7%90%86%402x.png", true, "人力资源管理" },
                    { new Guid("0190c3ef-8743-7713-a03d-061ba4f41c8a"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%86%9C%E4%B8%9A%E7%BB%8F%E6%B5%8E%402x.png", true, "农业经济" },
                    { new Guid("0190c3ef-8743-7896-bbc5-bfa6493ccc9a"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E7%9F%A5%E8%AF%86%E4%BA%A7%E6%9D%83%402x.png", true, "知识产权" },
                    { new Guid("0190c3ef-8743-792c-aa5b-cdb702201ebf"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%B7%A5%E5%95%86%E7%AE%A1%E7%90%86%402x.png", true, "工商管理" },
                    { new Guid("0190c3ef-8743-79bf-8e03-4716c24dacd5"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7983-997a-1141082743be"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E8%BF%90%E8%BE%93%E7%BB%8F%E6%B5%8E%402x.png", true, "运输经济" },
                    { new Guid("0190c3ef-8743-7b4c-aa7f-5ffe2e771de3"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E4%BA%BA%E5%8A%9B%E8%B5%84%E6%BA%90%E7%AE%A1%E7%90%86%402x.png", true, "人力资源管理" },
                    { new Guid("0190c3ef-8743-7b68-a903-3b525bfd3493"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7983-997a-1141082743be"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E4%BF%9D%E9%99%A9%402x.png", true, "保险" },
                    { new Guid("0190c3ef-8743-7bf8-abb8-737d392178ea"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7983-997a-1141082743be"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E4%BA%BA%E5%8A%9B%E8%B5%84%E6%BA%90%E7%AE%A1%E7%90%86%402x.png", true, "人力资源管理" },
                    { new Guid("0190c3ef-8743-7d1e-a683-0e4d8d0c2afa"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7983-997a-1141082743be"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%86%9C%E4%B8%9A%E7%BB%8F%E6%B5%8E%402x.png", true, "农业经济" },
                    { new Guid("0190c3ef-8743-7d39-9a41-6a8f593fbaf8"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7983-997a-1141082743be"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E8%B4%A2%E6%94%BF%E7%A8%8E%E6%94%B6%402x.png", true, "财政税收" },
                    { new Guid("0190c3ef-8743-7db1-b3d5-f308bedd33bb"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E9%87%91%E8%9E%8D%402x.png", true, "金融" },
                    { new Guid("0190c3ef-8743-7dbc-b823-906196e0f02a"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E4%BF%9D%E9%99%A9%402x.png", true, "保险" },
                    { new Guid("0190c3ef-8743-7dd6-89a9-946d858a5700"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%BB%BA%E7%AD%91%E4%B8%8E%E6%88%BF%E5%9C%B0%E4%BA%A7%402x.png", true, "建筑与房地产经济" },
                    { new Guid("0190c3ef-8743-7ec9-a5e4-0b49f9d39509"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7c1b-8630-a47ee4e91c09"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E6%97%85%E6%B8%B8%402x.png", true, "旅游经济" },
                    { new Guid("0190c3ef-8744-7093-aacf-5178a34306f3"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E5%86%9C%E4%B8%9A%E7%BB%8F%E6%B5%8E%402x.png", true, "农业经济" },
                    { new Guid("0190c3ef-8744-7164-8c80-bbd332272481"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E7%94%B5%E5%AD%90%E5%95%86%E5%8A%A1%E8%AE%BE%E8%AE%A1%E5%B8%88%402x.png", true, "电子商务设计师" },
                    { new Guid("0190c3ef-8744-721c-9f39-fdd3b2581276"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7cb5-9613-122673d29071"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/sg/%E7%A4%BE%E4%BC%9A%E5%B7%A5%E4%BD%9C%E5%AE%9E%E5%8A%A1%402x.png", true, "社会工作实务" },
                    { new Guid("0190c3ef-8744-7263-a310-8aa4054d75e2"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E4%BF%A1%E6%81%AF%E7%B3%BB%E7%BB%9F%E7%9B%91%E7%90%86%E5%B8%88%402x.png", true, "信息系统监理师" },
                    { new Guid("0190c3ef-8744-732a-81a4-b368da5f182c"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E8%BD%AF%E4%BB%B6%E8%AF%84%E6%B5%8B%E5%B8%88%402x.png", true, "软件评测师" },
                    { new Guid("0190c3ef-8744-73d8-963f-8f0535f7f43c"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E7%BD%91%E7%BB%9C%E5%B7%A5%E7%A8%8B%E5%B8%88%402x.png", true, "网络工程师" },
                    { new Guid("0190c3ef-8744-73e5-94cd-8fcacb2d3f54"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7f93-ad52-3bc70bd9dcce"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E7%B3%BB%E7%BB%9F%E5%88%86%E6%9E%90%E5%B8%88%402x.png", true, "系统分析师" },
                    { new Guid("0190c3ef-8744-74f3-b994-f6e9c30b86a7"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7b3c-b30f-4626b1f94b76"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/sg/%E7%A4%BE%E4%BC%9A%E5%B7%A5%E4%BD%9C%E7%BB%BC%E5%90%88%E8%83%BD%E5%8A%9B%402x.png", true, "社会工作综合能力" },
                    { new Guid("0190c3ef-8744-7539-8d73-27d88bdda825"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7d10-9754-339536267635"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E5%AE%A1%E8%AE%A1%402x.png", true, "审计" },
                    { new Guid("0190c3ef-8744-7566-9fd5-8c07537131da"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E7%9F%A5%E8%AF%86%E4%BA%A7%E6%9D%83%402x.png", true, "知识产权" },
                    { new Guid("0190c3ef-8744-75e7-b691-7db6e6f67170"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E8%BD%AF%E4%BB%B6%E8%AE%BE%E8%AE%A1%E5%B8%88%402x.png", true, "软件设计师" },
                    { new Guid("0190c3ef-8744-76b3-b54f-79dff76044b4"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7cb5-9613-122673d29071"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/sg/%E7%A4%BE%E4%BC%9A%E5%B7%A5%E4%BD%9C%E7%BB%BC%E5%90%88%E8%83%BD%E5%8A%9B%402x.png", true, "社会工作综合能力" },
                    { new Guid("0190c3ef-8744-777a-b985-0adeeb41e0cf"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7d10-9754-339536267635"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E7%BB%8F%E6%B5%8E%E6%B3%95%402x.png", true, "经济法" },
                    { new Guid("0190c3ef-8744-7805-8748-b1a7d4bb554b"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7b3c-b30f-4626b1f94b76"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/sg/%E7%A4%BE%E4%BC%9A%E5%B7%A5%E4%BD%9C%E6%B3%95%E8%A7%84%E4%B8%8E%E6%94%BF%E7%AD%96%402x.png", true, "社会工作法规与政策" },
                    { new Guid("0190c3ef-8744-7930-86f6-6afbf0b1e5e9"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E6%95%B0%E6%8D%AE%E5%BA%93%E7%B3%BB%E7%BB%9F%E5%B7%A5%E7%A8%8B%E5%B8%88%402x.png", true, "数据库系统工程师" },
                    { new Guid("0190c3ef-8744-7953-9b9e-036d64f9d722"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7d10-9754-339536267635"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E5%85%AC%E5%8F%B8%E6%88%98%E7%95%A5%E4%B8%8E%E9%A3%8E%E9%99%A9%E7%AE%A1%E7%90%86%402x.png", true, "公司战略与风险管理" },
                    { new Guid("0190c3ef-8744-7992-aa0c-e2e8efb7be8b"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E8%BF%90%E8%BE%93%E7%BB%8F%E6%B5%8E%402x.png", true, "运输经济" },
                    { new Guid("0190c3ef-8744-799c-a00f-1764d9c593cc"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7d10-9754-339536267635"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E8%B4%A2%E5%8A%A1%E6%88%90%E6%9C%AC%E7%AE%A1%E7%90%86%402x.png", true, "财务成本管理" },
                    { new Guid("0190c3ef-8744-79bf-869c-914549f73938"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7efd-8d62-8a061fe71b22"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E4%BC%9A%E8%AE%A1%E5%AE%9E%E5%8A%A1%402x.png", true, "中级会计实务" },
                    { new Guid("0190c3ef-8744-79f6-98d1-fa17543852f0"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7f93-ad52-3bc70bd9dcce"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E4%BF%A1%E6%81%AF%E7%B3%BB%E7%BB%9F%E9%A1%B9%E7%9B%AE%E7%AE%A1%E7%90%86%E5%B8%88%402x.png", true, "信息系统项目管理师" },
                    { new Guid("0190c3ef-8744-7adb-b2f6-165be84e2549"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7efd-8d62-8a061fe71b22"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E8%B4%A2%E5%8A%A1%E7%AE%A1%E7%90%86%402x.png", true, "财务管理" },
                    { new Guid("0190c3ef-8744-7bbe-a6e7-d3eabac9e5c3"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E7%B3%BB%E7%BB%9F%E9%9B%86%E6%88%90%E9%A1%B9%E7%9B%AE%E7%AE%A1%E7%90%86%E5%B7%A5%E7%A8%8B%E5%B8%88%402x.png", true, "系统集成项目管理工程师" },
                    { new Guid("0190c3ef-8744-7be0-8665-3cf3e4025b1c"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E4%BF%9D%E9%99%A9%402x.png", true, "保险" },
                    { new Guid("0190c3ef-8744-7c45-ab86-a8cede07a368"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7efd-8d62-8a061fe71b22"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E7%BB%8F%E6%B5%8E%E6%B3%95%E5%9F%BA%E7%A1%80%402x.png", true, "经济法" },
                    { new Guid("0190c3ef-8744-7c93-870a-bd345d0d9850"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7f93-ad52-3bc70bd9dcce"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E7%BD%91%E7%BB%9C%E8%A7%84%E8%AE%BE%E8%AE%A1%E5%B8%88%402x.png", true, "网络规划设计师" },
                    { new Guid("0190c3ef-8744-7da6-9939-cce429b59d47"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E6%97%85%E6%B8%B8%402x.png", true, "旅游经济" },
                    { new Guid("0190c3ef-8744-7e4f-a046-352818453d75"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7d10-9754-339536267635"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E4%BC%9A%E8%AE%A1%402x.png", true, "会计" },
                    { new Guid("0190c3ef-8744-7e71-a99b-1b31d3ccbf0c"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E4%BF%A1%E6%81%AF%E5%AE%89%E5%85%A8%E5%B7%A5%E7%A8%8B%E5%B8%88%402x.png", true, "信息安全工程师" },
                    { new Guid("0190c3ef-8744-7f14-8aec-9a55efe68c2f"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7b3c-b30f-4626b1f94b76"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/sg/%E7%A4%BE%E4%BC%9A%E5%B7%A5%E4%BD%9C%E5%AE%9E%E5%8A%A1%402x.png", true, "社会工作实务" },
                    { new Guid("0190c3ef-8744-7f15-be59-7c9aa57db72d"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-71ce-ba7d-6c26e155cc17"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E4%BF%A1%E6%81%AF%E7%B3%BB%E7%BB%9F%E7%AE%A1%E7%90%86%E5%B7%A5%E7%A8%8B%E5%B8%88%402x.png", true, "信息系统管理工程师" },
                    { new Guid("0190c3ef-8744-7f81-bce2-4ae76d963b81"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7fe3-ac55-f79166ebc5c9"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/jjs/%E8%B4%A2%E6%94%BF%E7%A8%8E%E6%94%B6%402x.png", true, "财政税收" },
                    { new Guid("0190c3ef-8744-7fde-9122-582cd15a8fda"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7d10-9754-339536267635"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/kj/%E7%A8%8E%E6%B3%95%402x.png", true, "税法" },
                    { new Guid("0190c3ef-8745-747f-8e86-91784dcde76f"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7f93-ad52-3bc70bd9dcce"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E7%B3%BB%E7%BB%9F%E8%A7%84%E5%88%92%E4%B8%8E%E7%AE%A1%E7%90%86%E5%B8%88%402x.png", true, "系统规划与管理师" },
                    { new Guid("0190c3ef-8745-783a-b8ed-f68787eeff35"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new Guid("0190c3e7-a245-7f93-ad52-3bc70bd9dcce"), "https://new-haokao.oss-cn-shenzhen.aliyuncs.com/CorrectionNotebook/rk/%E7%B3%BB%E7%BB%9F%E6%9E%B6%E6%9E%84%E8%AE%BE%E8%AE%A1%E5%B8%88%402x.png", true, "系统架构师" }
                });

            migrationBuilder.InsertData(
                table: "SubjectSort",
                columns: new[] { "Id", "CreatorId", "IsBuiltIn", "Priority", "SubjectId" },
                values: new object[,]
                {
                    { new Guid("0190ca74-1c44-7005-b5fd-17475dc6fa10"), new Guid("00000000-0000-0000-0000-000000000000"), true, 2, new Guid("0190c3ef-8743-792c-aa5b-cdb702201ebf") },
                    { new Guid("0190ca74-1c44-7065-8ceb-4391642e4644"), new Guid("00000000-0000-0000-0000-000000000000"), true, 2, new Guid("0190c3ef-8743-7bf8-abb8-737d392178ea") },
                    { new Guid("0190ca74-1c44-7109-be8c-5194444d7617"), new Guid("00000000-0000-0000-0000-000000000000"), true, 2, new Guid("0190c3ef-8743-76d3-be78-dbbbbc64b9da") },
                    { new Guid("0190ca74-1c44-71b7-b1ac-916c5244bc76"), new Guid("00000000-0000-0000-0000-000000000000"), true, 5, new Guid("0190c3ef-8743-76a6-8ce7-426d5e5e723f") },
                    { new Guid("0190ca74-1c44-71f8-8ca5-8edf01cadf27"), new Guid("00000000-0000-0000-0000-000000000000"), true, 5, new Guid("0190c3ef-8744-7566-9fd5-8c07537131da") },
                    { new Guid("0190ca74-1c44-72bf-8a4f-1f87045b5d04"), new Guid("00000000-0000-0000-0000-000000000000"), true, 10, new Guid("0190c3ef-8743-7598-899a-2f0d0283f097") },
                    { new Guid("0190ca74-1c44-72d0-b2af-1b12098ffb06"), new Guid("00000000-0000-0000-0000-000000000000"), true, 6, new Guid("0190c3ef-8743-7896-bbc5-bfa6493ccc9a") },
                    { new Guid("0190ca74-1c44-7301-ada3-fcefc48bfcd4"), new Guid("00000000-0000-0000-0000-000000000000"), true, 7, new Guid("0190c3ef-8743-7071-bb77-c3c115726963") },
                    { new Guid("0190ca74-1c44-73a7-af2d-9d42b8c79951"), new Guid("00000000-0000-0000-0000-000000000000"), true, 11, new Guid("0190c3ef-8743-7dbc-b823-906196e0f02a") },
                    { new Guid("0190ca74-1c44-73bc-b9b9-cd4f459cb77e"), new Guid("00000000-0000-0000-0000-000000000000"), true, 10, new Guid("0190c3ef-8743-7ec9-a5e4-0b49f9d39509") },
                    { new Guid("0190ca74-1c44-743b-b266-647862085453"), new Guid("00000000-0000-0000-0000-000000000000"), true, 4, new Guid("0190c3ef-8743-7170-8305-8f0c4ec6710f") },
                    { new Guid("0190ca74-1c44-7551-9dc1-94709d479d7b"), new Guid("00000000-0000-0000-0000-000000000000"), true, 9, new Guid("0190c3ef-8744-7da6-9939-cce429b59d47") },
                    { new Guid("0190ca74-1c44-7573-a642-80193a552399"), new Guid("00000000-0000-0000-0000-000000000000"), true, 3, new Guid("0190c3ef-8743-762e-8650-f21b638463b4") },
                    { new Guid("0190ca74-1c44-776b-8c42-a3dcf2887f3f"), new Guid("00000000-0000-0000-0000-000000000000"), true, 7, new Guid("0190c3ef-8744-7992-aa0c-e2e8efb7be8b") },
                    { new Guid("0190ca74-1c44-7808-95ab-2c84298ce396"), new Guid("00000000-0000-0000-0000-000000000000"), true, 6, new Guid("0190c3ef-8743-7033-832f-f3e23ef3be39") },
                    { new Guid("0190ca74-1c44-7827-a73a-33bdcff42b25"), new Guid("00000000-0000-0000-0000-000000000000"), true, 9, new Guid("0190c3ef-8743-7713-a03d-061ba4f41c8a") },
                    { new Guid("0190ca74-1c44-782a-a835-a3c3073874c0"), new Guid("00000000-0000-0000-0000-000000000000"), true, 8, new Guid("0190c3ef-8743-79bf-8e03-4716c24dacd5") },
                    { new Guid("0190ca74-1c44-783f-9ccf-4f2586e3bc58"), new Guid("00000000-0000-0000-0000-000000000000"), true, 7, new Guid("0190c3ef-8743-7d39-9a41-6a8f593fbaf8") },
                    { new Guid("0190ca74-1c44-78c8-b51d-68ec0bbf8336"), new Guid("00000000-0000-0000-0000-000000000000"), true, 8, new Guid("0190c3ef-8743-72ff-bbb7-1e179ec221ba") },
                    { new Guid("0190ca74-1c44-7928-a2c0-1e454f1b3426"), new Guid("00000000-0000-0000-0000-000000000000"), true, 4, new Guid("0190c3ef-8743-72b6-b702-b47b64eb8a04") },
                    { new Guid("0190ca74-1c44-79c9-83fd-b778c6aca4c2"), new Guid("00000000-0000-0000-0000-000000000000"), true, 6, new Guid("0190c3ef-8744-7f81-bce2-4ae76d963b81") },
                    { new Guid("0190ca74-1c44-79cf-92cc-6eb720470724"), new Guid("00000000-0000-0000-0000-000000000000"), true, 11, new Guid("0190c3ef-8743-7b68-a903-3b525bfd3493") },
                    { new Guid("0190ca74-1c44-7b47-91b2-14cee948c838"), new Guid("00000000-0000-0000-0000-000000000000"), true, 5, new Guid("0190c3ef-8743-7316-80b9-3943ab0f4964") },
                    { new Guid("0190ca74-1c44-7b50-8af2-facbba282958"), new Guid("00000000-0000-0000-0000-000000000000"), true, 1, new Guid("0190c3ef-8743-71ce-968a-bb9140f63b3a") },
                    { new Guid("0190ca74-1c44-7b87-b099-5c44eb3f26f3"), new Guid("00000000-0000-0000-0000-000000000000"), true, 8, new Guid("0190c3ef-8744-7093-aacf-5178a34306f3") },
                    { new Guid("0190ca74-1c44-7c98-b5a9-8aa9aa3eba15"), new Guid("00000000-0000-0000-0000-000000000000"), true, 3, new Guid("0190c3ef-8743-7db1-b3d5-f308bedd33bb") },
                    { new Guid("0190ca74-1c44-7d10-8d81-40defafa7d3b"), new Guid("00000000-0000-0000-0000-000000000000"), true, 1, new Guid("0190c3ef-8743-7529-b37b-54200cf96680") },
                    { new Guid("0190ca74-1c44-7d13-bc87-331c5d8703f0"), new Guid("00000000-0000-0000-0000-000000000000"), true, 9, new Guid("0190c3ef-8743-7d1e-a683-0e4d8d0c2afa") },
                    { new Guid("0190ca74-1c44-7e6d-98c7-f8241d96ac44"), new Guid("00000000-0000-0000-0000-000000000000"), true, 3, new Guid("0190c3ef-8743-7656-a6d8-eb98549cedb5") },
                    { new Guid("0190ca74-1c44-7e83-b2b1-ca7d6164768a"), new Guid("00000000-0000-0000-0000-000000000000"), true, 4, new Guid("0190c3ef-8743-7dd6-89a9-946d858a5700") },
                    { new Guid("0190ca74-1c44-7ebd-9f42-4278026948cd"), new Guid("00000000-0000-0000-0000-000000000000"), true, 1, new Guid("0190c3ef-8743-7b4c-aa7f-5ffe2e771de3") },
                    { new Guid("0190ca74-1c45-70aa-8421-4ece42785e76"), new Guid("00000000-0000-0000-0000-000000000000"), true, 6, new Guid("0190c3ef-8744-7e71-a99b-1b31d3ccbf0c") },
                    { new Guid("0190ca74-1c45-711e-904d-e3e3cd98a053"), new Guid("00000000-0000-0000-0000-000000000000"), true, 1, new Guid("0190c3ef-8744-7c45-ab86-a8cede07a368") },
                    { new Guid("0190ca74-1c45-712d-a6b1-2bd194e1ea01"), new Guid("00000000-0000-0000-0000-000000000000"), true, 3, new Guid("0190c3ef-8744-79bf-869c-914549f73938") },
                    { new Guid("0190ca74-1c45-715e-a5a6-dae02cf42571"), new Guid("00000000-0000-0000-0000-000000000000"), true, 5, new Guid("0190c3ef-8744-799c-a00f-1764d9c593cc") },
                    { new Guid("0190ca74-1c45-71fb-a47e-243da0cf1940"), new Guid("00000000-0000-0000-0000-000000000000"), true, 9, new Guid("0190c3ef-8744-732a-81a4-b368da5f182c") },
                    { new Guid("0190ca74-1c45-72ad-bda2-cef3016b8c46"), new Guid("00000000-0000-0000-0000-000000000000"), true, 2, new Guid("0190c3ef-8744-777a-b985-0adeeb41e0cf") },
                    { new Guid("0190ca74-1c45-72f6-bda1-a3be237469b0"), new Guid("00000000-0000-0000-0000-000000000000"), true, 3, new Guid("0190c3ef-8744-7e4f-a046-352818453d75") },
                    { new Guid("0190ca74-1c45-7346-a26b-70123f5e67e2"), new Guid("00000000-0000-0000-0000-000000000000"), true, 4, new Guid("0190c3ef-8744-7164-8c80-bbd332272481") },
                    { new Guid("0190ca74-1c45-7357-8866-5d4ccc6ec2a6"), new Guid("00000000-0000-0000-0000-000000000000"), true, 10, new Guid("0190c3ef-8744-7be0-8665-3cf3e4025b1c") },
                    { new Guid("0190ca74-1c45-736c-8026-4bc5d657194f"), new Guid("00000000-0000-0000-0000-000000000000"), true, 2, new Guid("0190c3ef-8744-75e7-b691-7db6e6f67170") },
                    { new Guid("0190ca74-1c45-7386-bf89-3165cad1cfaa"), new Guid("00000000-0000-0000-0000-000000000000"), true, 2, new Guid("0190c3ef-8744-76b3-b54f-79dff76044b4") },
                    { new Guid("0190ca74-1c45-7427-a87f-8cceb68e1bad"), new Guid("00000000-0000-0000-0000-000000000000"), true, 1, new Guid("0190c3ef-8744-7953-9b9e-036d64f9d722") },
                    { new Guid("0190ca74-1c45-74f4-8436-871e3eb22072"), new Guid("00000000-0000-0000-0000-000000000000"), true, 7, new Guid("0190c3ef-8744-7263-a310-8aa4054d75e2") },
                    { new Guid("0190ca74-1c45-773a-a605-d073e7f4f0a2"), new Guid("00000000-0000-0000-0000-000000000000"), true, 2, new Guid("0190c3ef-8744-73e5-94cd-8fcacb2d3f54") },
                    { new Guid("0190ca74-1c45-7774-a130-c28f2775fea8"), new Guid("00000000-0000-0000-0000-000000000000"), true, 3, new Guid("0190c3ef-8744-74f3-b994-f6e9c30b86a7") },
                    { new Guid("0190ca74-1c45-77cc-96d9-e1525db6eeba"), new Guid("00000000-0000-0000-0000-000000000000"), true, 1, new Guid("0190c3ef-8744-7bbe-a6e7-d3eabac9e5c3") },
                    { new Guid("0190ca74-1c45-7853-8682-a99091882d94"), new Guid("00000000-0000-0000-0000-000000000000"), true, 6, new Guid("0190c3ef-8744-7539-8d73-27d88bdda825") },
                    { new Guid("0190ca74-1c45-788c-ae35-ba0197e0e35c"), new Guid("00000000-0000-0000-0000-000000000000"), true, 1, new Guid("0190c3ef-8744-79f6-98d1-fa17543852f0") },
                    { new Guid("0190ca74-1c45-78e7-a0c3-72ed0756fdc5"), new Guid("00000000-0000-0000-0000-000000000000"), true, 1, new Guid("0190c3ef-8744-7f14-8aec-9a55efe68c2f") },
                    { new Guid("0190ca74-1c45-78f8-b4e9-c23e59c00b38"), new Guid("00000000-0000-0000-0000-000000000000"), true, 2, new Guid("0190c3ef-8744-7adb-b2f6-165be84e2549") },
                    { new Guid("0190ca74-1c45-79a9-b73f-630e96eb52f9"), new Guid("00000000-0000-0000-0000-000000000000"), true, 2, new Guid("0190c3ef-8744-7805-8748-b1a7d4bb554b") },
                    { new Guid("0190ca74-1c45-7a8c-9466-ac1c7dde0c82"), new Guid("00000000-0000-0000-0000-000000000000"), true, 5, new Guid("0190c3ef-8744-73d8-963f-8f0535f7f43c") },
                    { new Guid("0190ca74-1c45-7b3d-af06-7aab27a0fbcc"), new Guid("00000000-0000-0000-0000-000000000000"), true, 3, new Guid("0190c3ef-8744-7930-86f6-6afbf0b1e5e9") },
                    { new Guid("0190ca74-1c45-7e00-b18f-7aefbcbe2f2a"), new Guid("00000000-0000-0000-0000-000000000000"), true, 5, new Guid("0190c3ef-8745-783a-b8ed-f68787eeff35") },
                    { new Guid("0190ca74-1c45-7e08-b3ca-b9ca1d016483"), new Guid("00000000-0000-0000-0000-000000000000"), true, 1, new Guid("0190c3ef-8744-721c-9f39-fdd3b2581276") },
                    { new Guid("0190ca74-1c45-7e1f-aae8-41718b5a67fa"), new Guid("00000000-0000-0000-0000-000000000000"), true, 8, new Guid("0190c3ef-8744-7f15-be59-7c9aa57db72d") },
                    { new Guid("0190ca74-1c45-7f04-ae8a-5d16bb2b85b8"), new Guid("00000000-0000-0000-0000-000000000000"), true, 4, new Guid("0190c3ef-8745-747f-8e86-91784dcde76f") },
                    { new Guid("0190ca74-1c45-7fd9-a220-c768f0ba99d1"), new Guid("00000000-0000-0000-0000-000000000000"), true, 3, new Guid("0190c3ef-8744-7c93-870a-bd345d0d9850") },
                    { new Guid("0190ca74-1c45-7ff1-8efe-b87c2f7b72f0"), new Guid("00000000-0000-0000-0000-000000000000"), true, 4, new Guid("0190c3ef-8744-7fde-9122-582cd15a8fda") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamLevel_CreatorId_IsBuiltIn",
                table: "ExamLevel",
                columns: new[] { "CreatorId", "IsBuiltIn" });

            migrationBuilder.CreateIndex(
                name: "IX_ExamLevel_ParentId",
                table: "ExamLevel",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_GenerationLog_QuestionId_CreatorId",
                table: "GenerationLog",
                columns: new[] { "QuestionId", "CreatorId" });

            migrationBuilder.CreateIndex(
                name: "IX_Question_CreateTime_CreatorId_MasteryDegree",
                table: "Question",
                columns: new[] { "CreateTime", "CreatorId", "MasteryDegree" });

            migrationBuilder.CreateIndex(
                name: "IX_Question_SubjectId",
                table: "Question",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTag_TagId",
                table: "QuestionTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_CreatorId_IsBuiltIn",
                table: "Subject",
                columns: new[] { "CreatorId", "IsBuiltIn" });

            migrationBuilder.CreateIndex(
                name: "IX_Subject_ExamLevelId",
                table: "Subject",
                column: "ExamLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectSort_CreatorId_SubjectId",
                table: "SubjectSort",
                columns: new[] { "CreatorId", "SubjectId" });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectSort_SubjectId",
                table: "SubjectSort",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_CreatorId",
                table: "Tag",
                column: "CreatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenerationLog");

            migrationBuilder.DropTable(
                name: "QuestionTag");

            migrationBuilder.DropTable(
                name: "SubjectSort");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "ExamLevel");
        }
    }
}
