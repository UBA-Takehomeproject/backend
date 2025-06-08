using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateFKAuthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Authors_ObjectId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Users_ObjectId",
                table: "Authors",
                column: "ObjectId",
                principalTable: "Users",
                principalColumn: "ObjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Users_ObjectId",
                table: "Authors");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Authors_ObjectId",
                table: "Users",
                column: "ObjectId",
                principalTable: "Authors",
                principalColumn: "ObjectId");
        }
    }
}
