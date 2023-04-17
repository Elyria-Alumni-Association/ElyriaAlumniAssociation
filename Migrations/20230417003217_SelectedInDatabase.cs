using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElyriaAlumniAssociation.Migrations
{
    public partial class SelectedInDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Selected",
                table: "Alumnus",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Selected",
                table: "Alumnus");
        }
    }
}
