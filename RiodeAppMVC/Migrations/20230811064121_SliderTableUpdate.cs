using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiodeAppMVC.Migrations
{
    public partial class SliderTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isLeft",
                table: "Sliders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isLeft",
                table: "Sliders");
        }
    }
}
