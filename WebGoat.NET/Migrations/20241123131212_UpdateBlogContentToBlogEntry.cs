using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebGoat.NET.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBlogContentToBlogEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content_Value",
                table: "BlogResponses",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Contents_Value",
                table: "BlogEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content_Value",
                table: "BlogResponses");

            migrationBuilder.DropColumn(
                name: "Contents_Value",
                table: "BlogEntries");
        }
    }
}
