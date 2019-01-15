using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CCIA.Data.Migrations
{
    public partial class moreappreferences2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "audit_cond_sample_size",
                table: "crops",
                nullable: true);
        }
    }
}
