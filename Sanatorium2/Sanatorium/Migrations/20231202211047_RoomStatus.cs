using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sanatorium.Migrations
{
    /// <inheritdoc />
    public partial class RoomStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RoomStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_StatusId",
                table: "Rooms",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomStatuses_StatusId",
                table: "Rooms",
                column: "StatusId",
                principalTable: "RoomStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomStatuses_StatusId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "RoomStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_StatusId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Rooms");
        }
    }
}
