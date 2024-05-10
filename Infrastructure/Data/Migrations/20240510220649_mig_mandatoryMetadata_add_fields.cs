using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tributech.SensorManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_mandatoryMetadata_add_fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "MandatoryMetadataItems");

            migrationBuilder.AddColumn<string>(
                name: "DefaultValue",
                table: "MandatoryMetadataItems",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "MandatoryMetadataItems",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultValue",
                table: "MandatoryMetadataItems");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "MandatoryMetadataItems");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "MandatoryMetadataItems",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }
    }
}
