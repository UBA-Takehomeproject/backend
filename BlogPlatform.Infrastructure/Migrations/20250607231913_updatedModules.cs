using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "photoURL",
                table: "Users",
                newName: "PhotoURL");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "photoURL",
                table: "Authors",
                newName: "PhotoURL");

            migrationBuilder.RenameColumn(
                name: "phoneNumber",
                table: "Authors",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Authors",
                newName: "Email");

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorsInfoObjectId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "role",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorsInfoObjectId",
                table: "Blogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "BlogPosts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorsInfoObjectId",
                table: "BlogPosts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BlogObjectId",
                table: "BlogPosts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuthorsInfoObjectId",
                table: "Users",
                column: "AuthorsInfoObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_AuthorsInfoObjectId",
                table: "Blogs",
                column: "AuthorsInfoObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_AuthorsInfoObjectId",
                table: "BlogPosts",
                column: "AuthorsInfoObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_BlogObjectId",
                table: "BlogPosts",
                column: "BlogObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_Authors_AuthorsInfoObjectId",
                table: "BlogPosts",
                column: "AuthorsInfoObjectId",
                principalTable: "Authors",
                principalColumn: "ObjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_Blogs_BlogObjectId",
                table: "BlogPosts",
                column: "BlogObjectId",
                principalTable: "Blogs",
                principalColumn: "ObjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Authors_AuthorsInfoObjectId",
                table: "Blogs",
                column: "AuthorsInfoObjectId",
                principalTable: "Authors",
                principalColumn: "ObjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Authors_AuthorsInfoObjectId",
                table: "Users",
                column: "AuthorsInfoObjectId",
                principalTable: "Authors",
                principalColumn: "ObjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_Authors_AuthorsInfoObjectId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_Blogs_BlogObjectId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Authors_AuthorsInfoObjectId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Authors_AuthorsInfoObjectId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AuthorsInfoObjectId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_AuthorsInfoObjectId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_AuthorsInfoObjectId",
                table: "BlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_BlogObjectId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "AuthorsInfoObjectId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AuthorsInfoObjectId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "AuthorsInfoObjectId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "BlogObjectId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "PhotoURL",
                table: "Users",
                newName: "photoURL");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "PhotoURL",
                table: "Authors",
                newName: "photoURL");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Authors",
                newName: "phoneNumber");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Authors",
                newName: "email");

            migrationBuilder.AlterColumn<string>(
                name: "Date",
                table: "BlogPosts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
