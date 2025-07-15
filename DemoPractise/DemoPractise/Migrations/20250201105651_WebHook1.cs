using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoPractise.Migrations
{
    /// <inheritdoc />
    public partial class WebHook1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebhooksDeliveryAttempts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubscriptionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseStatusCode = table.Column<int>(type: "int", nullable: true),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhooksDeliveryAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebhooksDeliveryAttempts_Webhooks_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Webhooks",
                        principalColumn: "SubscriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebhooksDeliveryAttempts_SubscriptionId",
                table: "WebhooksDeliveryAttempts",
                column: "SubscriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebhooksDeliveryAttempts");
        }
    }
}
