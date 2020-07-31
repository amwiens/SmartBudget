using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBudget.EntityFramework.Migrations
{
    public partial class AddedTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Payee = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    IsCleared = table.Column<bool>(nullable: false),
                    TransactionType = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    ChargedAccountId = table.Column<int>(nullable: true),
                    TargetAccountId = table.Column<int>(nullable: true),
                    AccountId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_ChargedAccountId",
                        column: x => x.ChargedAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_TargetAccountId",
                        column: x => x.TargetAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ChargedAccountId",
                table: "Transactions",
                column: "ChargedAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TargetAccountId",
                table: "Transactions",
                column: "TargetAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
