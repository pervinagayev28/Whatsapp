using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whatsapp.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersTb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImagePath = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessagesTb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    ToId = table.Column<int>(type: "int", nullable: true),
                    FromId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesTb", x => x.Id);
                    table.ForeignKey(
                        name: "CK_FromId_To_UserId",
                        column: x => x.FromId,
                        principalTable: "UsersTb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "CK_ToId_To_UserId",
                        column: x => x.ToId,
                        principalTable: "UsersTb",
                        principalColumn: "Id");
                  
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessagesTb_FromId",
                table: "MessagesTb",
                column: "FromId");

          

            migrationBuilder.CreateIndex(
                name: "IX_MessagesTb_ToId",
                table: "MessagesTb",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "Uniqe_Gmail_Constraint",
                table: "UsersTb",
                column: "Gmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Uniqe_Password_Constraint",
                table: "UsersTb",
                column: "Password",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessagesTb");

            migrationBuilder.DropTable(
                name: "UsersTb");
        }
    }
}
