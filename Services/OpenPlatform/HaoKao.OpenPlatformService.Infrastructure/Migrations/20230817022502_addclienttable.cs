using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.OpenPlatformService.Infrastructure.Migrations
{
    public partial class addclienttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessClient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ClientId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProtocolType = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequireClientSecret = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ClientName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientUri = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LogoUri = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequireConsent = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowRememberConsent = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AlwaysIncludeUserClaimsInIdToken = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RequirePkce = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowPlainTextPkce = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RequireRequestObject = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowAccessTokensViaBrowser = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FrontChannelLogoutUri = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FrontChannelLogoutSessionRequired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BackChannelLogoutUri = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BackChannelLogoutSessionRequired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowOfflineAccess = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IdentityTokenLifetime = table.Column<int>(type: "int", nullable: false),
                    AllowedIdentityTokenSigningAlgorithms = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessTokenLifetime = table.Column<int>(type: "int", nullable: false),
                    AuthorizationCodeLifetime = table.Column<int>(type: "int", nullable: false),
                    ConsentLifetime = table.Column<int>(type: "int", nullable: true),
                    AbsoluteRefreshTokenLifetime = table.Column<int>(type: "int", nullable: false),
                    SlidingRefreshTokenLifetime = table.Column<int>(type: "int", nullable: false),
                    RefreshTokenUsage = table.Column<int>(type: "int", nullable: false),
                    UpdateAccessTokenClaimsOnRefresh = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RefreshTokenExpiration = table.Column<int>(type: "int", nullable: false),
                    AccessTokenType = table.Column<int>(type: "int", nullable: false),
                    EnableLocalLogin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IncludeJwtId = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AlwaysSendClientClaims = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ClientClaimsPrefix = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PairWiseSubjectSalt = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastAccessed = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UserSsoLifetime = table.Column<int>(type: "int", nullable: true),
                    UserCodeType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeviceCodeLifetime = table.Column<int>(type: "int", nullable: false),
                    NonEditable = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessClient", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessClientClaim",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Type = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessClientClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessClientClaim_AccessClient_AccessClientId",
                        column: x => x.AccessClientId,
                        principalTable: "AccessClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessClientCorsOrigins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Origin = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessClientCorsOrigins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessClientCorsOrigins_AccessClient_AccessClientId",
                        column: x => x.AccessClientId,
                        principalTable: "AccessClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessClientGrantType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AccessClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GrantType = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessClientGrantType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessClientGrantType_AccessClient_AccessClientId",
                        column: x => x.AccessClientId,
                        principalTable: "AccessClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessClientIdPRestriction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Provider = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessClientIdPRestriction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessClientIdPRestriction_AccessClient_AccessClientId",
                        column: x => x.AccessClientId,
                        principalTable: "AccessClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessClientPostLogoutRedirectUri",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PostLogoutRedirectUri = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessClientPostLogoutRedirectUri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessClientPostLogoutRedirectUri_AccessClient_AccessClientId",
                        column: x => x.AccessClientId,
                        principalTable: "AccessClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessClientProperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Key = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessClientProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessClientProperties_AccessClient_AccessClientId",
                        column: x => x.AccessClientId,
                        principalTable: "AccessClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessClientRedirectUri",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RedirectUri = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessClientRedirectUri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessClientRedirectUri_AccessClient_AccessClientId",
                        column: x => x.AccessClientId,
                        principalTable: "AccessClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessClientScope",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Scope = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessClientScope", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessClientScope_AccessClient_AccessClientId",
                        column: x => x.AccessClientId,
                        principalTable: "AccessClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessClientSecret",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Expiration = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Type = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AccessClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessClientSecret", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessClientSecret_AccessClient_AccessClientId",
                        column: x => x.AccessClientId,
                        principalTable: "AccessClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"),
                columns: new[] { "CreateTime", "LastLoginTime" },
                values: new object[] { new DateTime(2023, 8, 17, 10, 25, 1, 403, DateTimeKind.Local).AddTicks(1110), new DateTime(2023, 8, 17, 10, 25, 1, 403, DateTimeKind.Local).AddTicks(1080) });

            migrationBuilder.CreateIndex(
                name: "IX_AccessClient_ClientId",
                table: "AccessClient",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessClientClaim_AccessClientId",
                table: "AccessClientClaim",
                column: "AccessClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessClientCorsOrigins_AccessClientId",
                table: "AccessClientCorsOrigins",
                column: "AccessClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessClientGrantType_AccessClientId",
                table: "AccessClientGrantType",
                column: "AccessClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessClientIdPRestriction_AccessClientId",
                table: "AccessClientIdPRestriction",
                column: "AccessClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessClientPostLogoutRedirectUri_AccessClientId",
                table: "AccessClientPostLogoutRedirectUri",
                column: "AccessClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessClientProperties_AccessClientId",
                table: "AccessClientProperties",
                column: "AccessClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessClientRedirectUri_AccessClientId",
                table: "AccessClientRedirectUri",
                column: "AccessClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessClientScope_AccessClientId",
                table: "AccessClientScope",
                column: "AccessClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessClientSecret_AccessClientId",
                table: "AccessClientSecret",
                column: "AccessClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessClientClaim");

            migrationBuilder.DropTable(
                name: "AccessClientCorsOrigins");

            migrationBuilder.DropTable(
                name: "AccessClientGrantType");

            migrationBuilder.DropTable(
                name: "AccessClientIdPRestriction");

            migrationBuilder.DropTable(
                name: "AccessClientPostLogoutRedirectUri");

            migrationBuilder.DropTable(
                name: "AccessClientProperties");

            migrationBuilder.DropTable(
                name: "AccessClientRedirectUri");

            migrationBuilder.DropTable(
                name: "AccessClientScope");

            migrationBuilder.DropTable(
                name: "AccessClientSecret");

            migrationBuilder.DropTable(
                name: "AccessClient");

            migrationBuilder.UpdateData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"),
                columns: new[] { "CreateTime", "LastLoginTime" },
                values: new object[] { new DateTime(2023, 6, 20, 18, 3, 55, 914, DateTimeKind.Local).AddTicks(2650), new DateTime(2023, 6, 20, 18, 3, 55, 914, DateTimeKind.Local).AddTicks(2640) });
        }
    }
}
