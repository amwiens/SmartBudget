using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBudget.EntityFramework.Migrations
{
    public partial class AddMainCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MainCategory",
                table: "Categories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainCategory",
                table: "Categories");
        }
    }
}