using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrewMate.Data.Migrations
{
    /// <inheritdoc />
    public partial class approvalnote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovalNote",
                table: "LeaveApplications",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalNote",
                table: "LeaveApplications");
        }
    }
}
