using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B1WPFTask.Migrations.BankDB
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InputBalances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Passive = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputBalances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InputFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutputBalances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Passive = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputBalances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Turnovers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnovers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankAccountClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InputFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccountClasses_InputFiles_InputFileId",
                        column: x => x.InputFileId,
                        principalTable: "InputFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InputBalanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OutputBalanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TurnoverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_BankAccountClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "BankAccountClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccounts_InputBalances_InputBalanceId",
                        column: x => x.InputBalanceId,
                        principalTable: "InputBalances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccounts_OutputBalances_OutputBalanceId",
                        column: x => x.OutputBalanceId,
                        principalTable: "OutputBalances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Turnovers_TurnoverId",
                        column: x => x.TurnoverId,
                        principalTable: "Turnovers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountClasses_InputFileId",
                table: "BankAccountClasses",
                column: "InputFileId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_ClassId",
                table: "BankAccounts",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_InputBalanceId",
                table: "BankAccounts",
                column: "InputBalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_OutputBalanceId",
                table: "BankAccounts",
                column: "OutputBalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_TurnoverId",
                table: "BankAccounts",
                column: "TurnoverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "BankAccountClasses");

            migrationBuilder.DropTable(
                name: "InputBalances");

            migrationBuilder.DropTable(
                name: "OutputBalances");

            migrationBuilder.DropTable(
                name: "Turnovers");

            migrationBuilder.DropTable(
                name: "InputFiles");
        }
    }
}
