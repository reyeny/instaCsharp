using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabWork_59_Nyssanov_Yernar.Migrations
{
    /// <inheritdoc />
    public partial class _ChangeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FolowwingUserId",
                table: "Follows",
                newName: "FolowingUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FolowingUserId",
                table: "Follows",
                newName: "FolowwingUserId");
        }
    }
}
