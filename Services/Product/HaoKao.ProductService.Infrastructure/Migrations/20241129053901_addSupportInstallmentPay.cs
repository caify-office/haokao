using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaoKao.ProductService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addSupportInstallmentPay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSupportInstallmentPayment",
                table: "ProductPackage",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                comment: "是否支持分期支付");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSupportInstallmentPayment",
                table: "ProductPackage");
        }
    }
}
