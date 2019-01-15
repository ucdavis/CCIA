using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CCIA.Data.Migrations
{
    public partial class moreappreferences4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applications_abbrev_class_produced_class_produced_id",
                table: "applications");

            migrationBuilder.AddColumn<DateTime>(
                name: "date_certified",
                table: "var_full",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "tblname",
                table: "var_full",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_abbrev_class_produced",
                table: "applications",
                column: "class_produced_id",
                principalTable: "abbrev_class_produced",
                principalColumn: "class_produced_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
