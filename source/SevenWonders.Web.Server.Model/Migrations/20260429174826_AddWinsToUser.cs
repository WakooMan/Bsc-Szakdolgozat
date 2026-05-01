using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SevenWonders.Web.Server.Model.Migrations
{
    /// <inheritdoc />
    public partial class AddWinsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompetitiveWins",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompetitiveWins",
                table: "AspNetUsers");
        }
    }
}
