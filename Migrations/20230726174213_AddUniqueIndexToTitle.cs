using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCorePress.Migrations
{
    public partial class AddUniqueIndexToTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Posts_Title",
                table: "Posts",
                column: "Title",
                unique: true,
                filter: "[Title] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_Title",
                table: "Posts");
        }
    }
}
