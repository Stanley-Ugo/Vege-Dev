using Microsoft.EntityFrameworkCore.Migrations;

namespace Vega.Migrations
{
    public partial class AddSeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Make (Name) VALUES ('Make1')");
            migrationBuilder.Sql("INSERT INTO Make (Name) VALUES ('Make2')");
            migrationBuilder.Sql("INSERT INTO Make (Name) VALUES ('Make3')");

            migrationBuilder.Sql("INSERT INTO MODELS (Name, MakeId) VALUES ('Make1-ModelA', (SELECT ID FROM Make WHERE Name = 'Make1'))");
            migrationBuilder.Sql("INSERT INTO MODELS (Name, MakeId) VALUES ('Make1-ModelB', (SELECT ID FROM Make WHERE Name = 'Make1'))");
            migrationBuilder.Sql("INSERT INTO MODELS (Name, MakeId) VALUES ('Make1-ModelC', (SELECT ID FROM Make WHERE Name = 'Make1'))");

            migrationBuilder.Sql("INSERT INTO MODELS (Name, MakeId) VALUES ('Make2-ModelA', (SELECT ID FROM Make WHERE Name = 'Make2'))");
            migrationBuilder.Sql("INSERT INTO MODELS (Name, MakeId) VALUES ('Make2-ModelB', (SELECT ID FROM Make WHERE Name = 'Make2'))");
            migrationBuilder.Sql("INSERT INTO MODELS (Name, MakeId) VALUES ('Make2-ModelC', (SELECT ID FROM Make WHERE Name = 'Make2'))");

            migrationBuilder.Sql("INSERT INTO MODELS (Name, MakeId) VALUES ('Make3-ModelA', (SELECT ID FROM Make WHERE Name = 'Make3'))");
            migrationBuilder.Sql("INSERT INTO MODELS (Name, MakeId) VALUES ('Make3-ModelB', (SELECT ID FROM Make WHERE Name = 'Make3'))");
            migrationBuilder.Sql("INSERT INTO MODELS (Name, MakeId) VALUES ('Make3-ModelC', (SELECT ID FROM Make WHERE Name = 'Make3'))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Make WHERE Name IN ('Make1', 'Make2', 'Make3')");
        }
    }
}
