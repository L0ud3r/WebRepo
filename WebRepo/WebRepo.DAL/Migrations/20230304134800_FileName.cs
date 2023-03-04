using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRepo.App.Migrations
{
    /// <inheritdoc />
    public partial class FileName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "FileBlob",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "FileBlob");
        }
    }
}
