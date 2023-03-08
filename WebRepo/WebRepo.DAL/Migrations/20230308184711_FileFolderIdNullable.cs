using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRepo.App.Migrations
{
    /// <inheritdoc />
    public partial class FileFolderIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_VirtualDirectories_VirtualDirectoryId",
                table: "Files");

            migrationBuilder.AlterColumn<int>(
                name: "VirtualDirectoryId",
                table: "Files",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AlterColumn<int>(
                name: "VirtualDirectoryId",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_VirtualDirectories_VirtualDirectoryId",
                table: "Files",
                column: "VirtualDirectoryId",
                principalTable: "VirtualDirectories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
