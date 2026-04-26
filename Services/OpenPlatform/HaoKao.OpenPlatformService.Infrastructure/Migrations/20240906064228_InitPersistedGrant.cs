using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.OpenPlatformService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitPersistedGrant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersistedGrant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Key = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubjectId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SessionId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ConsumedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Data = table.Column<string>(type: "longtext", maxLength: 50000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrant", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AccessClient",
                keyColumn: "Id",
                keyValue: new Guid("2c939d03-564d-cfb5-7175-7065defd9dfb"),
                columns: new[] { "CreateTime", "UpdateTime" },
                values: new object[] { new DateTime(2024, 9, 6, 14, 42, 28, 335, DateTimeKind.Local).AddTicks(1630), new DateTime(2024, 9, 6, 14, 42, 28, 335, DateTimeKind.Local).AddTicks(1640) });

            migrationBuilder.UpdateData(
                table: "AccessClient",
                keyColumn: "Id",
                keyValue: new Guid("a17c159c-3385-f479-e0bf-de9e9d480cfa"),
                columns: new[] { "CreateTime", "UpdateTime" },
                values: new object[] { new DateTime(2024, 9, 6, 14, 42, 28, 335, DateTimeKind.Local).AddTicks(1690), new DateTime(2024, 9, 6, 14, 42, 28, 335, DateTimeKind.Local).AddTicks(1690) });

            migrationBuilder.UpdateData(
                table: "AccessClient",
                keyColumn: "Id",
                keyValue: new Guid("bfbe1287-a91e-2c4b-7159-295168c17040"),
                columns: new[] { "CreateTime", "UpdateTime" },
                values: new object[] { new DateTime(2024, 9, 6, 14, 42, 28, 335, DateTimeKind.Local).AddTicks(1700), new DateTime(2024, 9, 6, 14, 42, 28, 335, DateTimeKind.Local).AddTicks(1700) });

            migrationBuilder.UpdateData(
                table: "AccessClient",
                keyColumn: "Id",
                keyValue: new Guid("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"),
                columns: new[] { "CreateTime", "UpdateTime" },
                values: new object[] { new DateTime(2024, 9, 6, 14, 42, 28, 335, DateTimeKind.Local).AddTicks(1710), new DateTime(2024, 9, 6, 14, 42, 28, 335, DateTimeKind.Local).AddTicks(1710) });

            migrationBuilder.UpdateData(
                table: "AccessClientSecret",
                keyColumn: "Id",
                keyValue: new Guid("39fe74c3-4dcd-2af3-045a-81d33138c27b"),
                column: "Created",
                value: new DateTime(2024, 9, 6, 6, 42, 28, 335, DateTimeKind.Utc).AddTicks(4920));

            migrationBuilder.UpdateData(
                table: "AccessClientSecret",
                keyColumn: "Id",
                keyValue: new Guid("6a8f88b5-4a49-4d32-56e6-c7198721f42e"),
                column: "Created",
                value: new DateTime(2024, 9, 6, 6, 42, 28, 335, DateTimeKind.Utc).AddTicks(4890));

            migrationBuilder.UpdateData(
                table: "AccessClientSecret",
                keyColumn: "Id",
                keyValue: new Guid("cc65aceb-46d3-39a5-6513-c3ad4285b5dc"),
                column: "Created",
                value: new DateTime(2024, 9, 6, 6, 42, 28, 335, DateTimeKind.Utc).AddTicks(4910));

            migrationBuilder.UpdateData(
                table: "AccessClientSecret",
                keyColumn: "Id",
                keyValue: new Guid("da2ebf42-a5c8-a4c7-f12f-0a0435b22516"),
                column: "Created",
                value: new DateTime(2024, 9, 6, 6, 42, 28, 335, DateTimeKind.Utc).AddTicks(4900));

            migrationBuilder.UpdateData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"),
                columns: new[] { "CreateTime", "LastLoginTime" },
                values: new object[] { new DateTime(2024, 9, 6, 14, 42, 28, 333, DateTimeKind.Local).AddTicks(9620), new DateTime(2024, 9, 6, 14, 42, 28, 333, DateTimeKind.Local).AddTicks(9610) });

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrant_ClientId_SessionId",
                table: "PersistedGrant",
                columns: new[] { "ClientId", "SessionId" });

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrant_Expiration",
                table: "PersistedGrant",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrant_Key",
                table: "PersistedGrant",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrant_SubjectId_ClientId_SessionId",
                table: "PersistedGrant",
                columns: new[] { "SubjectId", "ClientId", "SessionId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersistedGrant");

            migrationBuilder.UpdateData(
                table: "AccessClient",
                keyColumn: "Id",
                keyValue: new Guid("2c939d03-564d-cfb5-7175-7065defd9dfb"),
                columns: new[] { "CreateTime", "UpdateTime" },
                values: new object[] { new DateTime(2024, 8, 30, 10, 41, 25, 839, DateTimeKind.Local).AddTicks(8790), new DateTime(2024, 8, 30, 10, 41, 25, 839, DateTimeKind.Local).AddTicks(8800) });

            migrationBuilder.UpdateData(
                table: "AccessClient",
                keyColumn: "Id",
                keyValue: new Guid("a17c159c-3385-f479-e0bf-de9e9d480cfa"),
                columns: new[] { "CreateTime", "UpdateTime" },
                values: new object[] { new DateTime(2024, 8, 30, 10, 41, 25, 839, DateTimeKind.Local).AddTicks(8840), new DateTime(2024, 8, 30, 10, 41, 25, 839, DateTimeKind.Local).AddTicks(8840) });

            migrationBuilder.UpdateData(
                table: "AccessClient",
                keyColumn: "Id",
                keyValue: new Guid("bfbe1287-a91e-2c4b-7159-295168c17040"),
                columns: new[] { "CreateTime", "UpdateTime" },
                values: new object[] { new DateTime(2024, 8, 30, 10, 41, 25, 839, DateTimeKind.Local).AddTicks(8850), new DateTime(2024, 8, 30, 10, 41, 25, 839, DateTimeKind.Local).AddTicks(8850) });

            migrationBuilder.UpdateData(
                table: "AccessClient",
                keyColumn: "Id",
                keyValue: new Guid("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"),
                columns: new[] { "CreateTime", "UpdateTime" },
                values: new object[] { new DateTime(2024, 8, 30, 10, 41, 25, 839, DateTimeKind.Local).AddTicks(8860), new DateTime(2024, 8, 30, 10, 41, 25, 839, DateTimeKind.Local).AddTicks(8860) });

            migrationBuilder.UpdateData(
                table: "AccessClientSecret",
                keyColumn: "Id",
                keyValue: new Guid("39fe74c3-4dcd-2af3-045a-81d33138c27b"),
                column: "Created",
                value: new DateTime(2024, 8, 30, 2, 41, 25, 840, DateTimeKind.Utc).AddTicks(1330));

            migrationBuilder.UpdateData(
                table: "AccessClientSecret",
                keyColumn: "Id",
                keyValue: new Guid("6a8f88b5-4a49-4d32-56e6-c7198721f42e"),
                column: "Created",
                value: new DateTime(2024, 8, 30, 2, 41, 25, 840, DateTimeKind.Utc).AddTicks(1300));

            migrationBuilder.UpdateData(
                table: "AccessClientSecret",
                keyColumn: "Id",
                keyValue: new Guid("cc65aceb-46d3-39a5-6513-c3ad4285b5dc"),
                column: "Created",
                value: new DateTime(2024, 8, 30, 2, 41, 25, 840, DateTimeKind.Utc).AddTicks(1320));

            migrationBuilder.UpdateData(
                table: "AccessClientSecret",
                keyColumn: "Id",
                keyValue: new Guid("da2ebf42-a5c8-a4c7-f12f-0a0435b22516"),
                column: "Created",
                value: new DateTime(2024, 8, 30, 2, 41, 25, 840, DateTimeKind.Utc).AddTicks(1310));

            migrationBuilder.UpdateData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"),
                columns: new[] { "CreateTime", "LastLoginTime" },
                values: new object[] { new DateTime(2024, 8, 30, 10, 41, 25, 838, DateTimeKind.Local).AddTicks(8730), new DateTime(2024, 8, 30, 10, 41, 25, 838, DateTimeKind.Local).AddTicks(8720) });
        }
    }
}
