using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CCIA.Data.Migrations
{
    public partial class continitialsetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "grower_id",
                table: "applications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "field_elevation",
                table: "applications",
                type: "int",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);
            
           
            
            migrationBuilder.CreateIndex(
                name: "IX_applications_applicant_id",
                table: "applications",
                column: "applicant_id");

            migrationBuilder.CreateIndex(
                name: "IX_applications_grower_id",
                table: "applications",
                column: "grower_id");
            

            migrationBuilder.CreateIndex(
                name: "IX_Organization_address_id",
                table: "Organization",
                column: "address_id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_applications_Organization_applicant_id",
            //    table: "applications",
            //    column: "applicant_id",
            //    principalTable: "Organization",
            //    principalColumn: "org_id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_applications_Organization_grower_id",
            //    table: "applications",
            //    column: "grower_id",
            //    principalTable: "Organization",
            //    principalColumn: "org_id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applications_Organization_applicant_id",
                table: "applications");

            migrationBuilder.DropForeignKey(
                name: "FK_applications_Organization_grower_id",
                table: "applications");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_applications_applicant_id",
                table: "applications");

            migrationBuilder.DropIndex(
                name: "IX_applications_grower_id",
                table: "applications");

            migrationBuilder.AlterColumn<int>(
                name: "grower_id",
                table: "applications",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "field_elevation",
                table: "applications",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
