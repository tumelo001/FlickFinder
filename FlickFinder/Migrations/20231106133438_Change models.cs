using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlickFinder.Migrations
{
    /// <inheritdoc />
    public partial class Changemodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "WatchList",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "WatchList");
        }
    }
}
