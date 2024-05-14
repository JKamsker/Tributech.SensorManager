using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tributech.SensorManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_mandatoryMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Sensors",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MandatoryMetadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SensorType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MandatoryMetadata", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MandatoryMetadataItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    MandatoryMetadataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MandatoryMetadataItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MandatoryMetadataItem_MandatoryMetadata_MandatoryMetadataId",
                        column: x => x.MandatoryMetadataId,
                        principalTable: "MandatoryMetadata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MandatoryMetadataItem_MandatoryMetadataId",
                table: "MandatoryMetadataItem",
                column: "MandatoryMetadataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MandatoryMetadataItem");

            migrationBuilder.DropTable(
                name: "MandatoryMetadata");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Sensors");
        }
    }
}
