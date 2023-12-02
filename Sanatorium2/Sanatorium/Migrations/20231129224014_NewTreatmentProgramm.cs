using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sanatorium.Migrations
{
    /// <inheritdoc />
    public partial class NewTreatmentProgramm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TreatmentPrograms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "TreatmentPrograms",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TreatmentPrograms");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "TreatmentPrograms");
        }
    }
}
