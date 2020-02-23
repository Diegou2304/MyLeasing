using Microsoft.EntityFrameworkCore.Migrations;

namespace MyLeasing.Web.Migrations
{
    public partial class InitialDb5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyType_PropertyTypeId",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyType",
                table: "PropertyType");

            migrationBuilder.RenameTable(
                name: "PropertyType",
                newName: "PropertyTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyTypes",
                table: "PropertyTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeId",
                table: "Properties",
                column: "PropertyTypeId",
                principalTable: "PropertyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeId",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyTypes",
                table: "PropertyTypes");

            migrationBuilder.RenameTable(
                name: "PropertyTypes",
                newName: "PropertyType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyType",
                table: "PropertyType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyType_PropertyTypeId",
                table: "Properties",
                column: "PropertyTypeId",
                principalTable: "PropertyType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
