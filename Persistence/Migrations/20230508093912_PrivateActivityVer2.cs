using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PrivateActivityVer2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isPrivate",
                table: "ActivityAttendees",
                newName: "IsPrivate");

            migrationBuilder.RenameColumn(
                name: "isPrivate",
                table: "Activities",
                newName: "IsPrivate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPrivate",
                table: "ActivityAttendees",
                newName: "isPrivate");

            migrationBuilder.RenameColumn(
                name: "IsPrivate",
                table: "Activities",
                newName: "isPrivate");
        }
    }
}
