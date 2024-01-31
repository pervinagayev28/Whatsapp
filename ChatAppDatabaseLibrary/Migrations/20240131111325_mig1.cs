using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppDatabaseLibrary.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersTbs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTbs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConnectionsTb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromId = table.Column<int>(type: "int", nullable: false),
                    ToId = table.Column<int>(type: "int", nullable: false),
                    SofDeleteFrom = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SoftDeleteTo = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionsTb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectionsTb_UsersTbs_FromId",
                        column: x => x.FromId,
                        principalTable: "UsersTbs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConnectionsTb_UsersTbs_ToId",
                        column: x => x.ToId,
                        principalTable: "UsersTbs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    ToId = table.Column<int>(type: "int", nullable: false),
                    FromId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_UsersTbs_FromId",
                        column: x => x.FromId,
                        principalTable: "UsersTbs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_UsersTbs_ToId",
                        column: x => x.ToId,
                        principalTable: "UsersTbs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionsTb_FromId",
                table: "ConnectionsTb",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionsTb_ToId",
                table: "ConnectionsTb",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_FromId",
                table: "Message",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ToId",
                table: "Message",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersTbs_Gmail",
                table: "UsersTbs",
                column: "Gmail",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectionsTb");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "UsersTbs");
        }
    }
}
