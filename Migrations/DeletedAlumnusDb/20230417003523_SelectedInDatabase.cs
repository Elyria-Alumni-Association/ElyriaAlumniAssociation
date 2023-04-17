using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElyriaAlumniAssociation.Migrations.DeletedAlumnusDb
{
    public partial class SelectedInDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Selected",
                table: "DeletedAlumnus",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Selected",
                table: "DeletedAlumnus");
        }
    }
}
