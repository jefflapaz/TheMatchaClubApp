using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheMatchaClub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAppearanceSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VatAmount",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "IsDarkMode",
                table: "StoreSettings",
                newName: "RequireCashCountOnClose");

            migrationBuilder.AddColumn<bool>(
                name: "AutoGenerateZReport",
                table: "StoreSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AutoLockQuickSaleIfNoSession",
                table: "StoreSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CashierName",
                table: "StoreSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrentOperatingLocation",
                table: "StoreSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CustomerTierFrequentMin",
                table: "StoreSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "CustomerTierFrequentSpend",
                table: "StoreSettings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CustomerTierLoyalMin",
                table: "StoreSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerTierRegularMin",
                table: "StoreSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "DefaultStartingCash",
                table: "StoreSettings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "EnableOverShortWarnings",
                table: "StoreSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PopupLocationName",
                table: "StoreSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceiptFooterMessage",
                table: "StoreSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceiptPaperWidth",
                table: "StoreSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ReceiptShowCashierName",
                table: "StoreSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ReceiptShowCustomerName",
                table: "StoreSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ReceiptShowOrderType",
                table: "StoreSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ReceiptShowSessionNumber",
                table: "StoreSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SessionTimeoutMinutes",
                table: "StoreSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SmtpPassword",
                table: "StoreSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SmtpPort",
                table: "StoreSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SmtpServer",
                table: "StoreSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "CashTendered",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CashierName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ChangeGiven",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "SessionId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "StoreSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AutoGenerateZReport", "AutoLockQuickSaleIfNoSession", "CashierName", "CurrentOperatingLocation", "CustomerTierFrequentMin", "CustomerTierFrequentSpend", "CustomerTierLoyalMin", "CustomerTierRegularMin", "DefaultStartingCash", "EnableOverShortWarnings", "PopupLocationName", "ReceiptFooterMessage", "ReceiptPaperWidth", "ReceiptShowCashierName", "ReceiptShowCustomerName", "ReceiptShowOrderType", "ReceiptShowSessionNumber", "RequireCashCountOnClose", "SessionTimeoutMinutes", "SmtpPassword", "SmtpPort", "SmtpServer" },
                values: new object[] { true, true, "", "", 16, 7500m, 8, 2, 200m, true, "", "Thank you for your purchase!", "80mm", true, true, true, false, true, 0, "", 587, "smtp.gmail.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoGenerateZReport",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "AutoLockQuickSaleIfNoSession",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "CashierName",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "CurrentOperatingLocation",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "CustomerTierFrequentMin",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "CustomerTierFrequentSpend",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "CustomerTierLoyalMin",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "CustomerTierRegularMin",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "DefaultStartingCash",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "EnableOverShortWarnings",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "PopupLocationName",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "ReceiptFooterMessage",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "ReceiptPaperWidth",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "ReceiptShowCashierName",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "ReceiptShowCustomerName",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "ReceiptShowOrderType",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "ReceiptShowSessionNumber",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "SessionTimeoutMinutes",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "SmtpPassword",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "SmtpPort",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "SmtpServer",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "CashTendered",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CashierName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ChangeGiven",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "RequireCashCountOnClose",
                table: "StoreSettings",
                newName: "IsDarkMode");

            migrationBuilder.AddColumn<decimal>(
                name: "VatAmount",
                table: "Orders",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "StoreSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsDarkMode",
                value: false);
        }
    }
}
