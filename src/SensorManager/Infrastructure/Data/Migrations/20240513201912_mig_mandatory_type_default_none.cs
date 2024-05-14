using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tributech.SensorManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_mandatory_type_default_none : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "MandatoryMetadataItems",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "none",
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "MandatoryMetadataItems",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldDefaultValue: "none");
        }
    }
}
