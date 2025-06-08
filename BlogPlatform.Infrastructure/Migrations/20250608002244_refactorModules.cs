using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class refactorModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_Authors_ObjectId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_Blogs_ObjectId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Authors_ObjectId",
                table: "Blogs");

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorsInfoObjectId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorsInfoObjectId",
                table: "Blogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                onDelete: ReferentialAction.Restrict);

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
                onDelete: ReferentialAction.Restrict);
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
                name: "AuthorsInfoObjectId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "AuthorsInfoObjectId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "BlogObjectId",
                table: "BlogPosts");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_Authors_ObjectId",
                table: "BlogPosts",
                column: "ObjectId",
                principalTable: "Authors",
                principalColumn: "ObjectId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_Blogs_ObjectId",
                table: "BlogPosts",
                column: "ObjectId",
                principalTable: "Blogs",
                principalColumn: "ObjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Authors_ObjectId",
                table: "Blogs",
                column: "ObjectId",
                principalTable: "Authors",
                principalColumn: "ObjectId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
