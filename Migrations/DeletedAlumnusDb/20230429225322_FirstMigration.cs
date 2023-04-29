using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElyriaAlumniAssociation.Migrations.DeletedAlumnusDb
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeletedAlumnus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    MiddleInitial = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    LastNameAtGraduation = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    School = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GraduationYear = table.Column<int>(type: "int", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    State = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScholasticAward = table.Column<bool>(type: "bit", nullable: false),
                    Athletics = table.Column<bool>(type: "bit", nullable: false),
                    Theatre = table.Column<bool>(type: "bit", nullable: false),
                    Band = table.Column<bool>(type: "bit", nullable: false),
                    Choir = table.Column<bool>(type: "bit", nullable: false),
                    Clubs = table.Column<bool>(type: "bit", nullable: false),
                    ClassOfficer = table.Column<bool>(type: "bit", nullable: false),
                    ROTC = table.Column<bool>(type: "bit", nullable: false),
                    OtherActivities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Selected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedAlumnus", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeletedAlumnus");
        }
    }
}
