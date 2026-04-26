using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.OpenPlatformService.Infrastructure.Migrations
{
    public partial class addSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AccessClient",
                columns: new[] { "Id", "AbsoluteRefreshTokenLifetime", "AccessTokenLifetime", "AccessTokenType", "AllowAccessTokensViaBrowser", "AllowOfflineAccess", "AllowPlainTextPkce", "AllowRememberConsent", "AlwaysIncludeUserClaimsInIdToken", "AlwaysSendClientClaims", "AuthorizationCodeLifetime", "BackChannelLogoutSessionRequired", "BackChannelLogoutUri", "ClientClaimsPrefix", "ClientId", "ClientName", "ClientUri", "ConsentLifetime", "CreateTime", "Description", "DeviceCodeLifetime", "EnableLocalLogin", "Enabled", "FrontChannelLogoutSessionRequired", "FrontChannelLogoutUri", "IdentityTokenLifetime", "IncludeJwtId", "LogoUri", "PairWiseSubjectSalt", "ProtocolType", "RefreshTokenExpiration", "RefreshTokenUsage", "RequireClientSecret", "RequireConsent", "RequirePkce", "RequireRequestObject", "SlidingRefreshTokenLifetime", "UpdateAccessTokenClaimsOnRefresh", "UpdateTime", "UserCodeType", "UserSsoLifetime" },
                values: new object[,]
                {
                    { new Guid("2c939d03-564d-cfb5-7175-7065defd9dfb"), 2592000, 300, 0, true, true, false, true, false, false, 300, true, null, "client_", "acounts.haokao123.com.personal.client", "好考教育平台-个人中心", "https://accounts.haokao123.com/personal", null, new DateTime(2023, 8, 30, 10, 1, 18, 283, DateTimeKind.Local).AddTicks(3666), null, 300, true, true, true, null, 300, false, null, null, "oidc", 1, 1, true, false, true, false, 1296000, false, new DateTime(2023, 8, 30, 10, 1, 18, 283, DateTimeKind.Local).AddTicks(3671), null, null },
                    { new Guid("a17c159c-3385-f479-e0bf-de9e9d480cfa"), 2592000, 2592000, 0, true, true, false, true, false, false, 300, true, null, "client_", "haokao.questionbank.app.client", "经济师慧题库手机App", "https://accounts.haokao123.com/app", null, new DateTime(2023, 8, 30, 10, 1, 18, 283, DateTimeKind.Local).AddTicks(3724), null, 300, true, true, true, null, 300, false, null, null, "oidc", 1, 1, true, false, true, false, 1296000, false, new DateTime(2023, 8, 30, 10, 1, 18, 283, DateTimeKind.Local).AddTicks(3724), null, null },
                    { new Guid("bfbe1287-a91e-2c4b-7159-295168c17040"), 2592000, 2592000, 0, true, true, false, true, false, false, 300, true, null, "client_", "haokao.questionbank.wechat.mini.client", "经济师慧题库微信小程序", "https://accounts.haokao123.com/app", null, new DateTime(2023, 8, 30, 10, 1, 18, 283, DateTimeKind.Local).AddTicks(3736), null, 300, true, true, true, null, 300, false, null, null, "oidc", 1, 1, true, false, true, false, 1296000, false, new DateTime(2023, 8, 30, 10, 1, 18, 283, DateTimeKind.Local).AddTicks(3736), null, null },
                    { new Guid("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"), 2592000, 3600, 0, true, true, false, true, false, false, 300, true, null, "client_", "haokao123.com.web.client", "好考教育平台", "http://localhost:8088", null, new DateTime(2023, 8, 30, 10, 1, 18, 283, DateTimeKind.Local).AddTicks(3744), null, 300, true, true, true, null, 300, false, null, null, "oidc", 1, 1, true, false, true, false, 1296000, false, new DateTime(2023, 8, 30, 10, 1, 18, 283, DateTimeKind.Local).AddTicks(3745), null, null }
                });

            migrationBuilder.UpdateData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"),
                columns: new[] { "CreateTime", "LastLoginTime" },
                values: new object[] { new DateTime(2023, 8, 30, 10, 1, 18, 281, DateTimeKind.Local).AddTicks(5399), new DateTime(2023, 8, 30, 10, 1, 18, 281, DateTimeKind.Local).AddTicks(5398) });

            migrationBuilder.InsertData(
                table: "AccessClientGrantType",
                columns: new[] { "Id", "AccessClientId", "GrantType" },
                values: new object[,]
                {
                    { new Guid("009f86bb-217a-c407-4c19-1fa1f5cb13ed"), new Guid("2c939d03-564d-cfb5-7175-7065defd9dfb"), "implicit" },
                    { new Guid("0b7cf34e-419a-1f6a-04bb-c95ad4d66a70"), new Guid("a17c159c-3385-f479-e0bf-de9e9d480cfa"), "implicit" },
                    { new Guid("17b85628-7ec5-87a8-7136-fd05298f5269"), new Guid("2c939d03-564d-cfb5-7175-7065defd9dfb"), "password" },
                    { new Guid("1f85560a-3376-ba7c-a507-53a3c493e1a6"), new Guid("bfbe1287-a91e-2c4b-7159-295168c17040"), "implicit" },
                    { new Guid("655fc79b-9570-0017-278b-d4621fe55158"), new Guid("a17c159c-3385-f479-e0bf-de9e9d480cfa"), "password" },
                    { new Guid("d781c6b6-4030-6ffc-a86e-48d8cc391607"), new Guid("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"), "implicit" },
                    { new Guid("ea6a14d4-41d6-0fc3-2b73-fd1816de8f96"), new Guid("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"), "password" }
                });

            migrationBuilder.InsertData(
                table: "AccessClientPostLogoutRedirectUri",
                columns: new[] { "Id", "AccessClientId", "PostLogoutRedirectUri" },
                values: new object[,]
                {
                    { new Guid("0e05e819-d9ec-cf01-a19e-ab511b3b4191"), new Guid("a17c159c-3385-f479-e0bf-de9e9d480cfa"), "https://accounts.haokao123.com/app" },
                    { new Guid("1cb77e01-1700-cbaf-d2da-397e1cb90029"), new Guid("2c939d03-564d-cfb5-7175-7065defd9dfb"), "https://accounts.haokao123.com/personal" },
                    { new Guid("63853332-ebaf-0f21-cb10-7669710e3270"), new Guid("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"), "http://localhost:8088" },
                    { new Guid("8629cdef-0b9e-91a5-8c15-088982c4e413"), new Guid("bfbe1287-a91e-2c4b-7159-295168c17040"), "https://accounts.haokao123.com/app" }
                });

            migrationBuilder.InsertData(
                table: "AccessClientRedirectUri",
                columns: new[] { "Id", "AccessClientId", "RedirectUri" },
                values: new object[,]
                {
                    { new Guid("1c19262f-631c-b9d3-902d-138e71bf0074"), new Guid("2c939d03-564d-cfb5-7175-7065defd9dfb"), "https://accounts.haokao123.com/personal/oidc/redirect-silentrenew" },
                    { new Guid("3daa2b34-874b-55d0-617a-79cd92c2690c"), new Guid("bfbe1287-a91e-2c4b-7159-295168c17040"), "https://accounts.haokao123.com/app/oidc/signin-oidc" },
                    { new Guid("60b2cc68-a697-8daa-d7f0-201bc0467c88"), new Guid("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"), "http://localhost:8088/signinOidc" },
                    { new Guid("7e63b742-ccd2-c34a-5cc8-e04661521bbe"), new Guid("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"), "http://localhost:8088/redirectSilentreneww" },
                    { new Guid("a5afcb17-33eb-4e2c-814b-ccd09cdf40df"), new Guid("a17c159c-3385-f479-e0bf-de9e9d480cfa"), "https://accounts.haokao123.com/app/oidc/signin-oidc" },
                    { new Guid("b3c05bd8-9123-a64f-8451-23dde1e93269"), new Guid("a17c159c-3385-f479-e0bf-de9e9d480cfa"), "https://accounts.haokao123.com/app/oidc/redirect-silentrenew" },
                    { new Guid("d0aba72c-b9bc-aeaa-9896-896058cb995f"), new Guid("2c939d03-564d-cfb5-7175-7065defd9dfb"), "https://accounts.haokao123.com/personal/oidc/signin-oidc" },
                    { new Guid("ee580b0c-78b7-28cb-e0f6-b628a7084e68"), new Guid("bfbe1287-a91e-2c4b-7159-295168c17040"), "https://accounts.haokao123.com/app/oidc/redirect-silentrenew" }
                });

            migrationBuilder.InsertData(
                table: "AccessClientScope",
                columns: new[] { "Id", "AccessClientId", "Scope" },
                values: new object[,]
                {
                    { new Guid("1deeb5da-7c99-176c-3cdf-207ed1d1bc80"), new Guid("2c939d03-564d-cfb5-7175-7065defd9dfb"), "email" },
                    { new Guid("2801d56e-274e-af54-d220-e755f65c78c1"), new Guid("bfbe1287-a91e-2c4b-7159-295168c17040"), "openid" },
                    { new Guid("3eac9b91-17b6-7f99-ad8c-9afb425cf909"), new Guid("a17c159c-3385-f479-e0bf-de9e9d480cfa"), "offline_access" },
                    { new Guid("3ff9b328-8328-5113-fca2-0419ccccf86d"), new Guid("a17c159c-3385-f479-e0bf-de9e9d480cfa"), "profile" },
                    { new Guid("52c3e1fe-4488-6515-74e5-af30d96836c4"), new Guid("bfbe1287-a91e-2c4b-7159-295168c17040"), "apiservice" },
                    { new Guid("63027529-dcae-9dfa-ae48-07d315299b0b"), new Guid("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"), "openid" },
                    { new Guid("6607a211-8166-9e62-69c3-7a6190df0b3a"), new Guid("bfbe1287-a91e-2c4b-7159-295168c17040"), "profile" },
                    { new Guid("7595011c-7fad-2313-631d-49748284451d"), new Guid("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"), "phone" },
                    { new Guid("76f15039-662d-0bff-73a4-ba15e6538b7b"), new Guid("a17c159c-3385-f479-e0bf-de9e9d480cfa"), "phone" },
                    { new Guid("81776cff-fe59-2f0b-c5a2-8911ad1c0fcb"), new Guid("2c939d03-564d-cfb5-7175-7065defd9dfb"), "apiservice" },
                    { new Guid("9e556e4b-e3c1-d50e-2cff-937a66b1b38d"), new Guid("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"), "apiservice" },
                    { new Guid("ae2629df-5292-70b5-1355-fd49741ba1ac"), new Guid("bfbe1287-a91e-2c4b-7159-295168c17040"), "offline_access" },
                    { new Guid("bd30467a-b144-4377-cb96-0c9c6d0eb1ca"), new Guid("2c939d03-564d-cfb5-7175-7065defd9dfb"), "openid" },
                    { new Guid("c8a65d32-db3a-257d-8e4b-168a232dbbbf"), new Guid("2c939d03-564d-cfb5-7175-7065defd9dfb"), "phone" },
                    { new Guid("c955cf45-88b2-dc77-530f-3042a59128cb"), new Guid("a17c159c-3385-f479-e0bf-de9e9d480cfa"), "openid" },
                    { new Guid("d71b85a7-70a7-28b7-3b4c-e494af940b97"), new Guid("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"), "offline_access" },
                    { new Guid("d72257e1-e21c-a9a7-1753-6e1b1624d15d"), new Guid("a17c159c-3385-f479-e0bf-de9e9d480cfa"), "apiservice" },
                    { new Guid("d822e094-8445-091d-2cf8-eff3318e37cb"), new Guid("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"), "profile" },
                    { new Guid("ebbcca29-684f-1b08-0871-e7f141dc3132"), new Guid("2c939d03-564d-cfb5-7175-7065defd9dfb"), "profile" },
                    { new Guid("f6c3f7cb-f474-3647-4d05-b8b2b18d934b"), new Guid("bfbe1287-a91e-2c4b-7159-295168c17040"), "phone" }
                });

            migrationBuilder.InsertData(
                table: "AccessClientSecret",
                columns: new[] { "Id", "AccessClientId", "Created", "Description", "Expiration", "HashType", "Type", "Value" },
                values: new object[,]
                {
                    { new Guid("39fe74c3-4dcd-2af3-045a-81d33138c27b"), new Guid("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"), new DateTime(2023, 8, 30, 2, 1, 18, 283, DateTimeKind.Utc).AddTicks(7520), null, null, "Sha256", "SharedSecret", "41c76ec7-8d4e-4b92-a516-35104f2d9606" },
                    { new Guid("6a8f88b5-4a49-4d32-56e6-c7198721f42e"), new Guid("2c939d03-564d-cfb5-7175-7065defd9dfb"), new DateTime(2023, 8, 30, 2, 1, 18, 283, DateTimeKind.Utc).AddTicks(7449), null, null, "Sha256", "SharedSecret", "c20be2ef-8da4-4827-9671-35dcbb510441" },
                    { new Guid("cc65aceb-46d3-39a5-6513-c3ad4285b5dc"), new Guid("bfbe1287-a91e-2c4b-7159-295168c17040"), new DateTime(2023, 8, 30, 2, 1, 18, 283, DateTimeKind.Utc).AddTicks(7514), null, null, "Sha256", "SharedSecret", "e68d9858-5dfd-4e18-a160-bdf68c700bb0" },
                    { new Guid("da2ebf42-a5c8-a4c7-f12f-0a0435b22516"), new Guid("a17c159c-3385-f479-e0bf-de9e9d480cfa"), new DateTime(2023, 8, 30, 2, 1, 18, 283, DateTimeKind.Utc).AddTicks(7504), null, null, "Sha256", "SharedSecret", "dd155ff4-69cd-47e0-b102-d90d90d770ed" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccessClientGrantType",
                keyColumn: "Id",
                keyValue: new Guid("009f86bb-217a-c407-4c19-1fa1f5cb13ed"));

            migrationBuilder.DeleteData(
                table: "AccessClientGrantType",
                keyColumn: "Id",
                keyValue: new Guid("0b7cf34e-419a-1f6a-04bb-c95ad4d66a70"));

            migrationBuilder.DeleteData(
                table: "AccessClientGrantType",
                keyColumn: "Id",
                keyValue: new Guid("17b85628-7ec5-87a8-7136-fd05298f5269"));

            migrationBuilder.DeleteData(
                table: "AccessClientGrantType",
                keyColumn: "Id",
                keyValue: new Guid("1f85560a-3376-ba7c-a507-53a3c493e1a6"));

            migrationBuilder.DeleteData(
                table: "AccessClientGrantType",
                keyColumn: "Id",
                keyValue: new Guid("655fc79b-9570-0017-278b-d4621fe55158"));

            migrationBuilder.DeleteData(
                table: "AccessClientGrantType",
                keyColumn: "Id",
                keyValue: new Guid("d781c6b6-4030-6ffc-a86e-48d8cc391607"));

            migrationBuilder.DeleteData(
                table: "AccessClientGrantType",
                keyColumn: "Id",
                keyValue: new Guid("ea6a14d4-41d6-0fc3-2b73-fd1816de8f96"));

            migrationBuilder.DeleteData(
                table: "AccessClientPostLogoutRedirectUri",
                keyColumn: "Id",
                keyValue: new Guid("0e05e819-d9ec-cf01-a19e-ab511b3b4191"));

            migrationBuilder.DeleteData(
                table: "AccessClientPostLogoutRedirectUri",
                keyColumn: "Id",
                keyValue: new Guid("1cb77e01-1700-cbaf-d2da-397e1cb90029"));

            migrationBuilder.DeleteData(
                table: "AccessClientPostLogoutRedirectUri",
                keyColumn: "Id",
                keyValue: new Guid("63853332-ebaf-0f21-cb10-7669710e3270"));

            migrationBuilder.DeleteData(
                table: "AccessClientPostLogoutRedirectUri",
                keyColumn: "Id",
                keyValue: new Guid("8629cdef-0b9e-91a5-8c15-088982c4e413"));

            migrationBuilder.DeleteData(
                table: "AccessClientRedirectUri",
                keyColumn: "Id",
                keyValue: new Guid("1c19262f-631c-b9d3-902d-138e71bf0074"));

            migrationBuilder.DeleteData(
                table: "AccessClientRedirectUri",
                keyColumn: "Id",
                keyValue: new Guid("3daa2b34-874b-55d0-617a-79cd92c2690c"));

            migrationBuilder.DeleteData(
                table: "AccessClientRedirectUri",
                keyColumn: "Id",
                keyValue: new Guid("60b2cc68-a697-8daa-d7f0-201bc0467c88"));

            migrationBuilder.DeleteData(
                table: "AccessClientRedirectUri",
                keyColumn: "Id",
                keyValue: new Guid("7e63b742-ccd2-c34a-5cc8-e04661521bbe"));

            migrationBuilder.DeleteData(
                table: "AccessClientRedirectUri",
                keyColumn: "Id",
                keyValue: new Guid("a5afcb17-33eb-4e2c-814b-ccd09cdf40df"));

            migrationBuilder.DeleteData(
                table: "AccessClientRedirectUri",
                keyColumn: "Id",
                keyValue: new Guid("b3c05bd8-9123-a64f-8451-23dde1e93269"));

            migrationBuilder.DeleteData(
                table: "AccessClientRedirectUri",
                keyColumn: "Id",
                keyValue: new Guid("d0aba72c-b9bc-aeaa-9896-896058cb995f"));

            migrationBuilder.DeleteData(
                table: "AccessClientRedirectUri",
                keyColumn: "Id",
                keyValue: new Guid("ee580b0c-78b7-28cb-e0f6-b628a7084e68"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("1deeb5da-7c99-176c-3cdf-207ed1d1bc80"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("2801d56e-274e-af54-d220-e755f65c78c1"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("3eac9b91-17b6-7f99-ad8c-9afb425cf909"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("3ff9b328-8328-5113-fca2-0419ccccf86d"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("52c3e1fe-4488-6515-74e5-af30d96836c4"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("63027529-dcae-9dfa-ae48-07d315299b0b"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("6607a211-8166-9e62-69c3-7a6190df0b3a"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("7595011c-7fad-2313-631d-49748284451d"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("76f15039-662d-0bff-73a4-ba15e6538b7b"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("81776cff-fe59-2f0b-c5a2-8911ad1c0fcb"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("9e556e4b-e3c1-d50e-2cff-937a66b1b38d"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("ae2629df-5292-70b5-1355-fd49741ba1ac"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("bd30467a-b144-4377-cb96-0c9c6d0eb1ca"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("c8a65d32-db3a-257d-8e4b-168a232dbbbf"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("c955cf45-88b2-dc77-530f-3042a59128cb"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("d71b85a7-70a7-28b7-3b4c-e494af940b97"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("d72257e1-e21c-a9a7-1753-6e1b1624d15d"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("d822e094-8445-091d-2cf8-eff3318e37cb"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("ebbcca29-684f-1b08-0871-e7f141dc3132"));

            migrationBuilder.DeleteData(
                table: "AccessClientScope",
                keyColumn: "Id",
                keyValue: new Guid("f6c3f7cb-f474-3647-4d05-b8b2b18d934b"));

            migrationBuilder.DeleteData(
                table: "AccessClientSecret",
                keyColumn: "Id",
                keyValue: new Guid("39fe74c3-4dcd-2af3-045a-81d33138c27b"));

            migrationBuilder.DeleteData(
                table: "AccessClientSecret",
                keyColumn: "Id",
                keyValue: new Guid("6a8f88b5-4a49-4d32-56e6-c7198721f42e"));

            migrationBuilder.DeleteData(
                table: "AccessClientSecret",
                keyColumn: "Id",
                keyValue: new Guid("cc65aceb-46d3-39a5-6513-c3ad4285b5dc"));

            migrationBuilder.DeleteData(
                table: "AccessClientSecret",
                keyColumn: "Id",
                keyValue: new Guid("da2ebf42-a5c8-a4c7-f12f-0a0435b22516"));

            migrationBuilder.DeleteData(
                table: "AccessClient",
                keyColumn: "Id",
                keyValue: new Guid("2c939d03-564d-cfb5-7175-7065defd9dfb"));

            migrationBuilder.DeleteData(
                table: "AccessClient",
                keyColumn: "Id",
                keyValue: new Guid("a17c159c-3385-f479-e0bf-de9e9d480cfa"));

            migrationBuilder.DeleteData(
                table: "AccessClient",
                keyColumn: "Id",
                keyValue: new Guid("bfbe1287-a91e-2c4b-7159-295168c17040"));

            migrationBuilder.DeleteData(
                table: "AccessClient",
                keyColumn: "Id",
                keyValue: new Guid("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"));

            migrationBuilder.UpdateData(
                table: "RegisterUser",
                keyColumn: "Id",
                keyValue: new Guid("d2ead372-8c6a-4c52-9f17-9d1599f202f0"),
                columns: new[] { "CreateTime", "LastLoginTime" },
                values: new object[] { new DateTime(2023, 8, 22, 10, 35, 19, 428, DateTimeKind.Local).AddTicks(2640), new DateTime(2023, 8, 22, 10, 35, 19, 428, DateTimeKind.Local).AddTicks(2638) });
        }
    }
}
