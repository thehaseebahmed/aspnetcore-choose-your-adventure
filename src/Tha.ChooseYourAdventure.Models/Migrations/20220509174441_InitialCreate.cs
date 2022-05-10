using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tha.ChooseYourAdventure.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adventures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRootNode = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptionTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AdventureNodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserAdventureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adventures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adventures_Adventures_AdventureNodeId",
                        column: x => x.AdventureNodeId,
                        principalTable: "Adventures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserAdventures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdventureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAdventures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAdventures_Adventures_AdventureId",
                        column: x => x.AdventureId,
                        principalTable: "Adventures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAdventureSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdventureStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserAdventureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAdventureSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAdventureSteps_Adventures_AdventureStepId",
                        column: x => x.AdventureStepId,
                        principalTable: "Adventures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adventures_AdventureNodeId",
                table: "Adventures",
                column: "AdventureNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Adventures_UserAdventureId",
                table: "Adventures",
                column: "UserAdventureId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAdventures_AdventureId",
                table: "UserAdventures",
                column: "AdventureId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAdventureSteps_AdventureStepId",
                table: "UserAdventureSteps",
                column: "AdventureStepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adventures_UserAdventures_UserAdventureId",
                table: "Adventures",
                column: "UserAdventureId",
                principalTable: "UserAdventures",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adventures_UserAdventures_UserAdventureId",
                table: "Adventures");

            migrationBuilder.DropTable(
                name: "UserAdventureSteps");

            migrationBuilder.DropTable(
                name: "UserAdventures");

            migrationBuilder.DropTable(
                name: "Adventures");
        }
    }
}
