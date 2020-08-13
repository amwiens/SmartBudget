using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBudget.EntityFramework.Migrations
{
    public partial class AddNoteToExpense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Expenses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Expenses");
        }
    }
}