using System;
using System.IO;
using System.Linq;
using TheMatchaClubApp.Core.Models;
using QuestPDF.Helpers;
using QuestPDF.Fluent;
using QuestInfrastructure = QuestPDF.Infrastructure;

namespace TheMatchaClubApp.Helpers
{
    public static class ZReportHelper
    {
        public static void GenerateZReportPdf(BusinessSession session, string filePath)
        {
            QuestPDF.Settings.License = QuestInfrastructure.LicenseType.Community;

            var items = Program.SessionService.GetAllItemSales(session.SessionId);
            var settings = Program.DataService.Settings;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.SegoeUI));

                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text(settings.StoreName).FontSize(20).Bold().FontColor("#52B743");
                                c.Item().Text("Z-REPORT (END OF SESSION)").FontSize(14).SemiBold().FontColor("#6B7280");
                            });

                            row.RelativeItem().AlignRight().Column(c =>
                            {
                                c.Item().Text($"Date: {session.OpenedAt:MMM dd, yyyy}").SemiBold();
                                c.Item().Text($"Session ID: {session.SessionId.ToString()[..8].ToUpper()}");
                                c.Item().Text($"Cashier: {session.OpenedBy}");
                                c.Item().Text($"Status: Closed").Bold().FontColor(Colors.Red.Medium);
                            });
                        });

                        col.Item().PaddingVertical(10).LineHorizontal(1).LineColor("#E5E7EB");
                    });

                    page.Content().Column(col =>
                    {
                        // Timing Info
                        col.Item().PaddingBottom(15).Row(row =>
                        {
                            row.RelativeItem().Text($"Opened: {session.OpenedAt:hh:mm tt}");
                            row.RelativeItem().AlignRight().Text($"Closed: {session.ClosedAt?.ToString("hh:mm tt") ?? "—"}");
                        });

                        // KPI Row
                        col.Item().PaddingVertical(10).Row(row =>
                        {
                            row.RelativeItem().Column(c => { c.Item().Text("STARTING CASH").FontSize(8).Bold().FontColor("#9CA3AF"); c.Item().Text($"₱{session.StartingCash:#,##0.00}").FontSize(14).Bold(); });
                            row.RelativeItem().Column(c => { c.Item().Text("EXPECTED CASH").FontSize(8).Bold().FontColor("#9CA3AF"); c.Item().Text($"₱{session.ExpectedCash:#,##0.00}").FontSize(14).Bold(); });
                            row.RelativeItem().Column(c => { c.Item().Text("ACTUAL CASH").FontSize(8).Bold().FontColor("#9CA3AF"); c.Item().Text($"₱{session.ActualCash:#,##0.00}").FontSize(14).Bold(); });
                            
                            decimal diff = session.ActualCash - session.ExpectedCash;
                            string diffLabel = diff >= 0 ? "OVER" : "SHORT";
                            string diffColor = diff >= 0 ? "#52B743" : "#EF4444";
                            row.RelativeItem().Column(c => { c.Item().Text(diffLabel).FontSize(8).Bold().FontColor("#9CA3AF"); c.Item().Text($"₱{diff:#,##0.00}").FontSize(14).Bold().FontColor(diffColor); });
                        });

                        col.Item().PaddingVertical(5).LineHorizontal(1).LineColor("#F3F4F6");

                        col.Item().PaddingVertical(10).Row(row =>
                        {
                            row.RelativeItem().Column(c => { c.Item().Text("TOTAL REVENUE").FontSize(8).Bold().FontColor("#9CA3AF"); c.Item().Text($"₱{session.TotalRevenue:#,##0.00}").FontSize(14).Bold(); });
                            row.RelativeItem().Column(c => { c.Item().Text("TRANSACTIONS").FontSize(8).Bold().FontColor("#9CA3AF"); c.Item().Text(session.TotalTransactions.ToString()).FontSize(14).Bold(); });
                            row.RelativeItem().Column(c => { c.Item().Text("UNITS SOLD").FontSize(8).Bold().FontColor("#9CA3AF"); c.Item().Text(session.TotalUnitsSold.ToString()).FontSize(14).Bold(); });
                        });

                        col.Item().PaddingVertical(10).LineHorizontal(1).LineColor("#E5E7EB");

                        // Products Table
                        col.Item().PaddingTop(15).Text("Product Performance Summary").FontSize(12).Bold();
                        col.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1.5f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyleHeader).Text("Product");
                                header.Cell().Element(CellStyleHeader).Text("Category");
                                header.Cell().Element(CellStyleHeader).AlignCenter().Text("Units");
                                header.Cell().Element(CellStyleHeader).AlignRight().Text("Revenue");

                                static QuestInfrastructure.IContainer CellStyleHeader(QuestInfrastructure.IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor("#E5E7EB");
                                }
                            });

                            foreach (var item in items.OrderByDescending(p => p.Revenue))
                            {
                                table.Cell().Element(CellStyleBody).Text(item.Name);
                                table.Cell().Element(CellStyleBody).Text(item.Category);
                                table.Cell().Element(CellStyleBody).AlignCenter().Text(item.Units.ToString());
                                table.Cell().Element(CellStyleBody).AlignRight().Text($"₱{item.Revenue:#,##0.00}");

                                static QuestInfrastructure.IContainer CellStyleBody(QuestInfrastructure.IContainer container)
                                {
                                    return container.PaddingVertical(5).BorderBottom(1).BorderColor("#F3F4F6");
                                }
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Generated by The Matcha Club POS • Z-Report").FontSize(8).FontColor("#9CA3AF");
                    });
                });
            }).GeneratePdf(filePath);
        }
    }
}
