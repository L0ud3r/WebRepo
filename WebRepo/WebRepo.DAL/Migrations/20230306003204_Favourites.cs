using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRepo.App.Migrations
{
    /// <inheritdoc />
    public partial class Favourites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilesCdn");

            migrationBuilder.AddColumn<bool>(
                name: "isFavourite",
                table: "Files",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isFavourite",
                table: "Files");

            migrationBuilder.CreateTable(
                name: "FilesCdn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CdnAccessKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CdnProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CdnRegion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CdnSecretKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CdnUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesCdn", x => x.Id);
                });
        }
    }
}
