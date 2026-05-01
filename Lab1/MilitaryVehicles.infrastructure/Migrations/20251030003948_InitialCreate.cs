using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilitaryVehicles.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Armies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrewMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Rank = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MilitaryVehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ArmyId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilitaryVehicles_Armies_ArmyId",
                        column: x => x.ArmyId,
                        principalTable: "Armies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AirVehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirVehicles_MilitaryVehicles_Id",
                        column: x => x.Id,
                        principalTable: "MilitaryVehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroundVehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroundVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroundVehicles_MilitaryVehicles_Id",
                        column: x => x.Id,
                        principalTable: "MilitaryVehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeaVehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeaVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeaVehicles_MilitaryVehicles_Id",
                        column: x => x.Id,
                        principalTable: "MilitaryVehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleCrewMembers",
                columns: table => new
                {
                    VehicleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CrewMemberId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleCrewMembers", x => new { x.VehicleId, x.CrewMemberId });
                    table.ForeignKey(
                        name: "FK_VehicleCrewMembers_CrewMembers_CrewMemberId",
                        column: x => x.CrewMemberId,
                        principalTable: "CrewMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleCrewMembers_MilitaryVehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "MilitaryVehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Helicopters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Speed = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Helicopters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Helicopters_AirVehicles_Id",
                        column: x => x.Id,
                        principalTable: "AirVehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tanks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Firepower = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tanks_GroundVehicles_Id",
                        column: x => x.Id,
                        principalTable: "GroundVehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Destroyers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Torpedoes = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destroyers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Destroyers_SeaVehicles_Id",
                        column: x => x.Id,
                        principalTable: "SeaVehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryVehicles_ArmyId",
                table: "MilitaryVehicles",
                column: "ArmyId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleCrewMembers_CrewMemberId",
                table: "VehicleCrewMembers",
                column: "CrewMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Destroyers");

            migrationBuilder.DropTable(
                name: "Helicopters");

            migrationBuilder.DropTable(
                name: "Tanks");

            migrationBuilder.DropTable(
                name: "VehicleCrewMembers");

            migrationBuilder.DropTable(
                name: "SeaVehicles");

            migrationBuilder.DropTable(
                name: "AirVehicles");

            migrationBuilder.DropTable(
                name: "GroundVehicles");

            migrationBuilder.DropTable(
                name: "CrewMembers");

            migrationBuilder.DropTable(
                name: "MilitaryVehicles");

            migrationBuilder.DropTable(
                name: "Armies");
        }
    }
}
