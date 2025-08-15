using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 

namespace WebApplication3.Migrations
{
 
    public partial class InitialCreate : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Klubi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ime = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Mesto = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SteviloSmucarjev = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klubi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Smucarji",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ime = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Priimek = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LetoRojstva = table.Column<int>(type: "INTEGER", nullable: false),
                    Drzava = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Smucarji", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmucarjiVKlubih",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SmucarId = table.Column<int>(type: "INTEGER", nullable: false),
                    KlubId = table.Column<int>(type: "INTEGER", nullable: false),
                    OdLeta = table.Column<int>(type: "INTEGER", nullable: false),
                    DoLeta = table.Column<int>(type: "INTEGER", nullable: false),
                    Tekmovanja = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmucarjiVKlubih", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmucarjiVKlubih_Klubi_KlubId",
                        column: x => x.KlubId,
                        principalTable: "Klubi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmucarjiVKlubih_Smucarji_SmucarId",
                        column: x => x.SmucarId,
                        principalTable: "Smucarji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Klubi",
                columns: new[] { "Id", "Ime", "Mesto", "SteviloSmucarjev" },
                values: new object[,]
                {
                    { 1, "Triglav", "Krajn", 14 },
                    { 2, "Everest", "Zagreb", 3 },
                    { 3, "Mountain", "Rim", 26 }
                });

            migrationBuilder.InsertData(
                table: "Smucarji",
                columns: new[] { "Id", "Drzava", "Ime", "LetoRojstva", "Priimek" },
                values: new object[,]
                {
                    { 1, "Nemcija", "Blaz", 2010, "Jurkovic" },
                    { 2, "Austrija", "Leo", 2000, "Puncec" },
                    { 3, "Hrvaska", "Ivan", 1918, "Borovickic" },
                    { 4, "Slovenija", "Denis", 1960, "Krajnc" }
                });

            migrationBuilder.InsertData(
                table: "SmucarjiVKlubih",
                columns: new[] { "Id", "DoLeta", "KlubId", "OdLeta", "SmucarId", "Tekmovanja" },
                values: new object[,]
                {
                    { 1, 2024, 1, 2021, 1, 1 },
                    { 2, 2020, 2, 2018, 1, 42 },
                    { 3, 2024, 1, 2019, 2, 24 },
                    { 4, 2024, 1, 2023, 3, 24 },
                    { 5, 2021, 3, 2007, 3, 6350 },
                    { 6, 2024, 3, 2020, 4, 23 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmucarjiVKlubih_KlubId",
                table: "SmucarjiVKlubih",
                column: "KlubId");

            migrationBuilder.CreateIndex(
                name: "IX_SmucarjiVKlubih_SmucarId",
                table: "SmucarjiVKlubih",
                column: "SmucarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmucarjiVKlubih");

            migrationBuilder.DropTable(
                name: "Klubi");

            migrationBuilder.DropTable(
                name: "Smucarji");
        }
    }
}
