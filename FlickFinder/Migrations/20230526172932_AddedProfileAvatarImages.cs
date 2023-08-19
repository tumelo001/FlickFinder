using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlickFinder.Migrations
{
    /// <inheritdoc />
    public partial class AddedProfileAvatarImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarImage",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "AvatarImageURL",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarImageURL",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<byte[]>(
                name: "AvatarImage",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
