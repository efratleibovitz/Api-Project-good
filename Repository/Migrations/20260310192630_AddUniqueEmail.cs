using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RATING",
                columns: table => new
                {
                    RATING_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HOST = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    METHOD = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    PATH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    REFERER = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    USER_AGENT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Record_Date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RATING", x => x.RATING_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserEmail",
                table: "Users",
                column: "UserEmail",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RATING");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserEmail",
                table: "Users");
        }
    }
}
