using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerBlazorIdentity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeDescriptionFromOliveRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "OliveRoles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OliveRoles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
