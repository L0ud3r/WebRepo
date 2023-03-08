using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRepo.App.Migrations
{
    /// <inheritdoc />
    public partial class FileToFolder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VirtualDirectoryId",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Files_VirtualDirectoryId",
                table: "Files",
                column: "VirtualDirectoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_VirtualDirectories_VirtualDirectoryId",
                table: "Files",
                column: "VirtualDirectoryId",
                principalTable: "VirtualDirectories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_VirtualDirectories_VirtualDirectoryId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_VirtualDirectoryId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "VirtualDirectoryId",
                table: "Files");
        }
    }
}
