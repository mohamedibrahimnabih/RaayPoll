using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaayPoll.API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditRecordToPollModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AuditRecord_CreatedAt",
                table: "Polls",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "AuditRecord_CreatedById",
                table: "Polls",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "AuditRecord_LastModifiedAt",
                table: "Polls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuditRecord_LastModifiedById",
                table: "Polls",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Polls_AuditRecord_CreatedById",
                table: "Polls",
                column: "AuditRecord_CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Polls_AuditRecord_LastModifiedById",
                table: "Polls",
                column: "AuditRecord_LastModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_AspNetUsers_AuditRecord_CreatedById",
                table: "Polls",
                column: "AuditRecord_CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_AspNetUsers_AuditRecord_LastModifiedById",
                table: "Polls",
                column: "AuditRecord_LastModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_AspNetUsers_AuditRecord_CreatedById",
                table: "Polls");

            migrationBuilder.DropForeignKey(
                name: "FK_Polls_AspNetUsers_AuditRecord_LastModifiedById",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Polls_AuditRecord_CreatedById",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Polls_AuditRecord_LastModifiedById",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "AuditRecord_CreatedAt",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "AuditRecord_CreatedById",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "AuditRecord_LastModifiedAt",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "AuditRecord_LastModifiedById",
                table: "Polls");
        }
    }
}
