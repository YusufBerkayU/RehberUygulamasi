using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RehberUygulamasi.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoPathToPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Photo",
                table: "Persons",
                newName: "PhotoPath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoPath",
                table: "Persons",
                newName: "Photo");
        }
    }
}
