using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Agoda.FileDownloaderSystem.DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Protocol",
                columns: table => new
                {
                    ProtocolId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Protocol", x => x.ProtocolId);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    StatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    FileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(nullable: true),
                    Destination = table.Column<string>(nullable: true),
                    DownloadStartedDate = table.Column<DateTime>(nullable: false),
                    DownloadEndedDate = table.Column<DateTime>(nullable: false),
                    IsLargeData = table.Column<bool>(nullable: false),
                    IsSlow = table.Column<bool>(nullable: false),
                    PercentageOfFailure = table.Column<int>(nullable: false),
                    ElapsedTime = table.Column<double>(nullable: false),
                    DownloadSpeed = table.Column<double>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    ProtocolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_File_Protocol_ProtocolId",
                        column: x => x.ProtocolId,
                        principalTable: "Protocol",
                        principalColumn: "ProtocolId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_File_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Protocol",
                columns: new[] { "ProtocolId", "Name" },
                values: new object[,]
                {
                    { 1, "http" },
                    { 2, "https" },
                    { 3, "ftp" },
                    { 4, "sftp" },
                    { 5, "net.tcp" },
                    { 6, "net.pipe" }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "StatusId", "Name" },
                values: new object[,]
                {
                    { 1, "Completed" },
                    { 2, "Failed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_ProtocolId",
                table: "File",
                column: "ProtocolId");

            migrationBuilder.CreateIndex(
                name: "IX_File_StatusId",
                table: "File",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "Protocol");

            migrationBuilder.DropTable(
                name: "Status");
        }
    }
}
