using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameBlog.Migrations
{
    /// <inheritdoc />
    public partial class AdjustValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                columns: new[] { "Content", "CreatedAt", "Title" },
                values: new object[] { "This is a post", new DateTime(2024, 5, 11, 16, 40, 29, 841, DateTimeKind.Local).AddTicks(4708), "Test Title" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1,
                columns: new[] { "Content", "CreatedAt", "Title" },
                values: new object[] { "System", new DateTime(2024, 5, 11, 16, 11, 54, 854, DateTimeKind.Local).AddTicks(3767), "System" });
        }
    }
}
