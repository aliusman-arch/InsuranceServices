using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceServices.Migrations
{
    /// <inheritdoc />
    public partial class AddPolicyNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanRequests_UserPolicies_UserPolicyPolicyId",
                table: "LoanRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PremiumPayments_UserPolicies_UserPolicyPolicyId",
                table: "PremiumPayments");

            migrationBuilder.DropColumn(
                name: "PremiumAmount",
                table: "UserPolicies");

            migrationBuilder.DropColumn(
                name: "SumAssured",
                table: "UserPolicies");

            migrationBuilder.AlterColumn<int>(
                name: "UserPolicyPolicyId",
                table: "PremiumPayments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UserPolicyPolicyId",
                table: "LoanRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanRequests_UserPolicies_UserPolicyPolicyId",
                table: "LoanRequests",
                column: "UserPolicyPolicyId",
                principalTable: "UserPolicies",
                principalColumn: "PolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PremiumPayments_UserPolicies_UserPolicyPolicyId",
                table: "PremiumPayments",
                column: "UserPolicyPolicyId",
                principalTable: "UserPolicies",
                principalColumn: "PolicyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanRequests_UserPolicies_UserPolicyPolicyId",
                table: "LoanRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PremiumPayments_UserPolicies_UserPolicyPolicyId",
                table: "PremiumPayments");

            migrationBuilder.AddColumn<decimal>(
                name: "PremiumAmount",
                table: "UserPolicies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SumAssured",
                table: "UserPolicies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "UserPolicyPolicyId",
                table: "PremiumPayments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserPolicyPolicyId",
                table: "LoanRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LoanRequests_UserPolicies_UserPolicyPolicyId",
                table: "LoanRequests",
                column: "UserPolicyPolicyId",
                principalTable: "UserPolicies",
                principalColumn: "PolicyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PremiumPayments_UserPolicies_UserPolicyPolicyId",
                table: "PremiumPayments",
                column: "UserPolicyPolicyId",
                principalTable: "UserPolicies",
                principalColumn: "PolicyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
