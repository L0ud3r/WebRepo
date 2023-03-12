using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRepo.App.Migrations
{
    /// <inheritdoc />
    public partial class UserProfilePhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "photoURL",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "photoURL",
                table: "Users");
        }
    }
}
