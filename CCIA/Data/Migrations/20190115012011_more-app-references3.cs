using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CCIA.Data.Migrations
{
    public partial class moreappreferences3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applications_var_full_selected_variety_id",
                table: "applications");

            migrationBuilder.DropTable(
                name: "var_full");

            migrationBuilder.DropIndex(
                name: "IX_applications_selected_variety_id",
                table: "applications");
        }
    }
}
