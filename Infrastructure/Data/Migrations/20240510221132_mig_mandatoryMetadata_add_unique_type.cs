using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tributech.SensorManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_mandatoryMetadata_add_unique_type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MandatoryMetadataItems_MandatoryMetadata_MandatoryMetadataId",
                table: "MandatoryMetadataItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MandatoryMetadata",
                table: "MandatoryMetadata");

            migrationBuilder.RenameTable(
                name: "MandatoryMetadata",
                newName: "MandatoryMetadatas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MandatoryMetadatas",
                table: "MandatoryMetadatas",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MandatoryMetadatas_SensorType",
                table: "MandatoryMetadatas",
                column: "SensorType",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MandatoryMetadataItems_MandatoryMetadatas_MandatoryMetadataId",
                table: "MandatoryMetadataItems",
                column: "MandatoryMetadataId",
                principalTable: "MandatoryMetadatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MandatoryMetadataItems_MandatoryMetadatas_MandatoryMetadataId",
                table: "MandatoryMetadataItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MandatoryMetadatas",
                table: "MandatoryMetadatas");

            migrationBuilder.DropIndex(
                name: "IX_MandatoryMetadatas_SensorType",
                table: "MandatoryMetadatas");

            migrationBuilder.RenameTable(
                name: "MandatoryMetadatas",
                newName: "MandatoryMetadata");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MandatoryMetadata",
                table: "MandatoryMetadata",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MandatoryMetadataItems_MandatoryMetadata_MandatoryMetadataId",
                table: "MandatoryMetadataItems",
                column: "MandatoryMetadataId",
                principalTable: "MandatoryMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
