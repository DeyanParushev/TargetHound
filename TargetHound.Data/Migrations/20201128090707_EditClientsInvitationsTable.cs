using Microsoft.EntityFrameworkCore.Migrations;

namespace TargetHound.Data.Migrations
{
    public partial class EditClientsInvitationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ClientInvitations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ClientInvitations");
        }
    }
}
