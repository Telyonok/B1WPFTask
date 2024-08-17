using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B1WPFTask.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RandomRows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LatinString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RussianString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PositiveInt = table.Column<int>(type: "int", nullable: false),
                    PositiveDouble = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RandomRows", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RandomRows");
        }
    }
}
