using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceServices.Migrations
{
    /// <inheritdoc />
    public partial class AddLoanRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoanAmount",
                table: "LoanRequests",
                newName: "RequestAmount");

            migrationBuilder.RenameColumn(
                name: "AdminComments",
                table: "LoanRequests",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "LoanId",
                table: "LoanRequests",
                newName: "LoanRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestAmount",
                table: "LoanRequests",
                newName: "LoanAmount");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "LoanRequests",
                newName: "AdminComments");

            migrationBuilder.RenameColumn(
                name: "LoanRequestId",
                table: "LoanRequests",
                newName: "LoanId");
        }
    }
}
