using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CCIA.Data.Migrations
{
    public partial class moreappreferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_applications_farm_county",
                table: "applications",
                column: "farm_county");

            migrationBuilder.AddForeignKey(
                name: "FK_applications_county_farm_county",
                table: "applications",
                column: "farm_county",
                principalTable: "county",
                principalColumn: "county_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applications_county_farm_county",
                table: "applications");

            migrationBuilder.DropIndex(
                name: "IX_applications_farm_county",
                table: "applications");
        }
    }
}
