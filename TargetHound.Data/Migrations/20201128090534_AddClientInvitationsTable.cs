using Microsoft.EntityFrameworkCore.Migrations;

namespace TargetHound.Data.Migrations
{
    public partial class AddClientInvitationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boreholes_Collars_CollarId",
                table: "Boreholes");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyPoints_Boreholes_BoreholeId",
                table: "SurveyPoints");

            migrationBuilder.CreateTable(
                name: "ClientInvitations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientInvitations_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientInvitations_ClientId",
                table: "ClientInvitations",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boreholes_Collars_CollarId",
                table: "Boreholes",
                column: "CollarId",
                principalTable: "Collars",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyPoints_Boreholes_BoreholeId",
                table: "SurveyPoints",
                column: "BoreholeId",
                principalTable: "Boreholes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boreholes_Collars_CollarId",
                table: "Boreholes");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyPoints_Boreholes_BoreholeId",
                table: "SurveyPoints");

            migrationBuilder.DropTable(
                name: "ClientInvitations");

            migrationBuilder.AddForeignKey(
                name: "FK_Boreholes_Collars_CollarId",
                table: "Boreholes",
                column: "CollarId",
                principalTable: "Collars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyPoints_Boreholes_BoreholeId",
                table: "SurveyPoints",
                column: "BoreholeId",
                principalTable: "Boreholes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
