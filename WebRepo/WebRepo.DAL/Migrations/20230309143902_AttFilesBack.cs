using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRepo.App.Migrations
{
    /// <inheritdoc />
    public partial class AttFilesBack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAgo",
                table: "Files");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedAgo",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
