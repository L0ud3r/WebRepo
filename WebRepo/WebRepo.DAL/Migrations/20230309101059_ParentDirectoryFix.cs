using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRepo.App.Migrations
{
    /// <inheritdoc />
    public partial class ParentDirectoryFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VirtualDirectories_VirtualDirectories_ParentDirectoryId",
                table: "VirtualDirectories");

            migrationBuilder.DropIndex(
                name: "IX_VirtualDirectories_ParentDirectoryId",
                table: "VirtualDirectories");

            migrationBuilder.RenameColumn(
                name: "ParentDirectoryId",
                table: "VirtualDirectories",
                newName: "ParentDirectory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentDirectory",
                table: "VirtualDirectories",
                newName: "ParentDirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualDirectories_ParentDirectoryId",
                table: "VirtualDirectories",
                column: "ParentDirectoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_VirtualDirectories_VirtualDirectories_ParentDirectoryId",
                table: "VirtualDirectories",
                column: "ParentDirectoryId",
                principalTable: "VirtualDirectories",
                principalColumn: "Id");
        }
    }
}
