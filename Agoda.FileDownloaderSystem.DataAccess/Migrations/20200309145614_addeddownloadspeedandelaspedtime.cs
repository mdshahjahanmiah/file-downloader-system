using Microsoft.EntityFrameworkCore.Migrations;

namespace Agoda.FileDownloaderSystem.DataAccess.Migrations
{
    public partial class addeddownloadspeedandelaspedtime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSlowOrFirst",
                table: "File");

            migrationBuilder.AddColumn<double>(
                name: "DownloadSpeed",
                table: "File",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ElapsedTime",
                table: "File",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSlow",
                table: "File",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadSpeed",
                table: "File");

            migrationBuilder.DropColumn(
                name: "ElapsedTime",
                table: "File");

            migrationBuilder.DropColumn(
                name: "IsSlow",
                table: "File");

            migrationBuilder.AddColumn<bool>(
                name: "IsSlowOrFirst",
                table: "File",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
