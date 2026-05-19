using System;
using System.IO;
using System.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClubApp.Core
{
    public static class ReceiptPdfGenerator
    {
        public static void Generate(Order order, StoreSettings settings, string filePath)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var accentColor = "#52B743";
            string location = ReceiptRenderer.GetDisplayLocation(settings);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    float widthMm = settings.ReceiptPaperWidth == "58mm" ? 58f : 80f;
                    float marginMm = settings.ReceiptPaperWidth == "58mm" ? 2f : 5f;
                    page.ContinuousSize(widthMm, Unit.Millimetre);
                    page.Margin(marginMm, Unit.Millimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily(Fonts.Verdana));

                    // ── Header ───────────────────────────────────
                    page.Header().Column(col =>
                    {
                        col.Item().Height(4).Background(accentColor);
                        col.Item().PaddingTop(10).AlignCenter().Text(t =>
                        {
                            t.Span("🍵 ").FontSize(18).FontColor(accentColor);
                            t.Span(settings.StoreName).FontSize(16).Bold().FontColor(accentColor);
                        });
                        col.Item().AlignCenter().Text(location).FontSize(8).FontColor(Colors.Grey.Medium);
                        col.Item().AlignCenter().Text($"{settings.Phone}  •  {settings.Email}").FontSize(8).FontColor(Colors.Grey.Medium);
                        col.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten3);
                    });

                    // ── Content ──────────────────────────────────
                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        // Order Metadata
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("ORDER ID").FontSize(7).Bold().FontColor(Colors.Grey.Medium);
                                c.Item().Text(order.OrderId).Bold().FontSize(10);
                            });
                            row.RelativeItem().AlignRight().Column(c =>
                            {
                                c.Item().Text("DATE").FontSize(7).Bold().FontColor(Colors.Grey.Medium);
                                c.Item().Text(order.Timestamp.ToString("dd MMM yyyy HH:mm")).FontSize(9);
                            });
                        });

                        col.Item().PaddingTop(8).Row(row =>
                        {
                            if (settings.ReceiptShowCustomerName)
                            {
                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("CUSTOMER").FontSize(7).Bold().FontColor(Colors.Grey.Medium);
                                    c.Item().Text(order.CustomerName ?? "Walk-In").FontSize(9);
                                });
                            }
                            if (settings.ReceiptShowOrderType)
                            {
                                row.RelativeItem().AlignRight().Column(c =>
                                {
                                    c.Item().Text("TYPE").FontSize(7).Bold().FontColor(Colors.Grey.Medium);
                                    c.Item().Text(order.OrderType ?? "Dine-In").FontSize(9);
                                });
                            }
                        });

                        if (settings.ReceiptShowCashierName)
                        {
                            col.Item().PaddingTop(4).Text(t =>
                            {
                                t.Span("Cashier: ").FontSize(7).FontColor(Colors.Grey.Medium);
                                t.Span(order.CashierName ?? Program.GetCurrentCashierName()).FontSize(8);
                            });
                        }

                        // Items Table
                        col.Item().PaddingTop(15).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().PaddingBottom(4).Text("ITEM").FontSize(8).Bold().FontColor(Colors.Grey.Darken2);
                                header.Cell().PaddingBottom(4).AlignCenter().Text("QTY").FontSize(8).Bold().FontColor(Colors.Grey.Darken2);
                                header.Cell().PaddingBottom(4).AlignRight().Text("TOTAL").FontSize(8).Bold().FontColor(Colors.Grey.Darken2);
                            });

                            foreach (var item in order.Items)
                            {
                                table.Cell().PaddingVertical(2).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten4).Text(item.ProductName).FontSize(8);
                                table.Cell().PaddingVertical(2).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten4).AlignCenter().Text(item.Quantity.ToString()).FontSize(8);
                                table.Cell().PaddingVertical(2).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten4).AlignRight().Text($"₱{item.LineTotal:#,##0.00}").FontSize(8);
                            }
                        });

                        // Totals
                        float valueColWidth = settings.ReceiptPaperWidth == "58mm" ? 50f : 80f;
                        col.Item().PaddingTop(10).AlignRight().Column(innerCol =>
                        {
                            innerCol.Item().Row(r =>
                            {
                                r.RelativeItem().Text("Subtotal").FontSize(8).FontColor(Colors.Grey.Medium);
                                r.ConstantItem(valueColWidth).AlignRight().Text($"₱{order.Subtotal:#,##0.00}").FontSize(8);
                            });

                            innerCol.Item().PaddingTop(4).Background(accentColor).PaddingHorizontal(8).PaddingVertical(4).Row(r =>
                            {
                                r.RelativeItem().Text("TOTAL").FontSize(11).Bold().FontColor(Colors.White);
                                r.ConstantItem(valueColWidth).AlignRight().Text($"₱{order.Total:#,##0.00}").FontSize(11).Bold().FontColor(Colors.White);
                            });
                        });

                        // Cash / Change
                        if (order.CashTendered > 0)
                        {
                            col.Item().PaddingTop(8).AlignRight().Column(innerCol =>
                            {
                                innerCol.Item().Row(r =>
                                {
                                    r.RelativeItem().Text("Cash Tendered").FontSize(8).FontColor(Colors.Grey.Medium);
                                    r.ConstantItem(valueColWidth).AlignRight().Text($"₱{order.CashTendered:#,##0.00}").FontSize(8);
                                });
                                innerCol.Item().Row(r =>
                                {
                                    r.RelativeItem().Text("Change").FontSize(8).Bold().FontColor(accentColor);
                                    r.ConstantItem(valueColWidth).AlignRight().Text($"₱{order.ChangeGiven:#,##0.00}").FontSize(8).Bold().FontColor(accentColor);
                                });
                            });
                        }

                        col.Item().PaddingTop(20).AlignCenter().Column(c =>
                        {
                            c.Item().Text($"Paid via {order.PaymentMethod}").FontSize(8).Bold().FontColor(Colors.Grey.Darken1);
                            if (settings.ReceiptShowCashierName)
                                c.Item().Text($"Served by {order.CashierName ?? Program.GetCurrentCashierName()}").FontSize(7).FontColor(Colors.Grey.Medium);
                        });
                    });

                    // ── Footer ───────────────────────────────────
                    page.Footer().PaddingTop(10).AlignCenter().Column(c =>
                    {
                        c.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten3);
                        string footer = settings.ReceiptFooterMessage;
                        if (string.IsNullOrWhiteSpace(footer)) footer = "Thank you for your purchase!";
                        c.Item().PaddingTop(5).Text(footer).FontSize(8).Italic().FontColor(Colors.Grey.Medium);
                    });
                });
            }).GeneratePdf(filePath);
        }
    }
}
