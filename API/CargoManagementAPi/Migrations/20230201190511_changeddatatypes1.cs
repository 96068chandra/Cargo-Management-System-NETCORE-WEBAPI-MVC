using Microsoft.EntityFrameworkCore.Migrations;

namespace CargoManagementAPi.Migrations
{
    public partial class changeddatatypes1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CargoOrderDetails_cargoStatuses_CargoStatusStatusId",
                table: "CargoOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_CargoOrderDetails_CargoStatusStatusId",
                table: "CargoOrderDetails");

            migrationBuilder.DropColumn(
                name: "CargoStatusStatusId",
                table: "CargoOrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "CargoStatusId",
                table: "CargoOrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CargoOrderDetails_CargoStatusId",
                table: "CargoOrderDetails",
                column: "CargoStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_CargoOrderDetails_cargoStatuses_CargoStatusId",
                table: "CargoOrderDetails",
                column: "CargoStatusId",
                principalTable: "cargoStatuses",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CargoOrderDetails_cargoStatuses_CargoStatusId",
                table: "CargoOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_CargoOrderDetails_CargoStatusId",
                table: "CargoOrderDetails");

            migrationBuilder.AlterColumn<string>(
                name: "CargoStatusId",
                table: "CargoOrderDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CargoStatusStatusId",
                table: "CargoOrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CargoOrderDetails_CargoStatusStatusId",
                table: "CargoOrderDetails",
                column: "CargoStatusStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_CargoOrderDetails_cargoStatuses_CargoStatusStatusId",
                table: "CargoOrderDetails",
                column: "CargoStatusStatusId",
                principalTable: "cargoStatuses",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
