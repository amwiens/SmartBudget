using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBudget.EntityFramework.Migrations
{
    public partial class AddedAvailableAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AvailableAmount",
                table: "Accounts",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableAmount",
                table: "Accounts");
        }
    }
}