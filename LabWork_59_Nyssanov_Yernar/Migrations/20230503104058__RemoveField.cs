using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabWork_59_Nyssanov_Yernar.Migrations
{
    /// <inheritdoc />
    public partial class _RemoveField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserNumber",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserNumber",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
