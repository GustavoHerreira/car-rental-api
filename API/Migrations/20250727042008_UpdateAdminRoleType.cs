using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminRoleType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove a operação automática de alteração de coluna e usa SQL explícito
            migrationBuilder.Sql(
                "ALTER TABLE \"Administrators\" ALTER COLUMN \"Role\" TYPE integer USING " +
                "CASE \"Role\" " +
                "WHEN 'admin' THEN 0 " +
                "WHEN 'Admin' THEN 0 " +
                "WHEN 'editor' THEN 1 " +
                "WHEN 'Editor' THEN 1 " +
                "ELSE 0 END");

            // Atualiza o valor do administrador padrão
            migrationBuilder.UpdateData(
                table: "Administrators",
                keyColumn: "Id",
                keyValue: 1,
                column: "Role",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove a operação automática e usa SQL explícito para reverter
            migrationBuilder.Sql(
                "ALTER TABLE \"Administrators\" ALTER COLUMN \"Role\" TYPE character varying(16) USING " +
                "CASE \"Role\" " +
                "WHEN 0 THEN 'Admin' " +
                "WHEN 1 THEN 'Editor' " +
                "ELSE 'Admin' END");

            // Reverte o valor do administrador padrão
            migrationBuilder.UpdateData(
                table: "Administrators",
                keyColumn: "Id",
                keyValue: 1,
                column: "Role",
                value: "admin");
        }
    }
}
