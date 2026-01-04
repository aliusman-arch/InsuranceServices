using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceServices.Migrations
{
    /// <inheritdoc />
    public partial class RenameSchemeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasePremium",
                table: "InsuranceSchemes");

            migrationBuilder.DropColumn(
                name: "MaxSumAssured",
                table: "InsuranceSchemes");

            migrationBuilder.RenameColumn(
                name: "MinSumAssured",
                table: "InsuranceSchemes",
                newName: "AmountLimit");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "InsuranceSchemes",
                newName: "SchemeType");

            migrationBuilder.AddColumn<int>(
                name: "TenureYears",
                table: "InsuranceSchemes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenureYears",
                table: "InsuranceSchemes");

            migrationBuilder.RenameColumn(
                name: "SchemeType",
                table: "InsuranceSchemes",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "AmountLimit",
                table: "InsuranceSchemes",
                newName: "MinSumAssured");

            migrationBuilder.AddColumn<decimal>(
                name: "BasePremium",
                table: "InsuranceSchemes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxSumAssured",
                table: "InsuranceSchemes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
