using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheMatchaClubApp.Core;
using TheMatchaClubApp.Core.Models;
using TheMatchaClubApp.Core.Services;

namespace TheMatchaClubApp.Helpers
{
    /// <summary>
    /// Provides local CSV export, full ZIP backup, and restore functionality.
    /// All operations are fully offline — no cloud, no external APIs.
    /// </summary>
    public static class BackupService
    {
        private static readonly string AppFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TheMatchaClub");

        private static readonly string DefaultBackupFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MatchaPOS", "Backups");

        private static readonly string DefaultExportFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MatchaPOS", "Exports");

        // ══════════════════════════════════════════════════════════════
        //  CSV EXPORTS
        // ══════════════════════════════════════════════════════════════

        /// <summary>
        /// Exports all orders to a CSV file. Returns the saved file path.
        /// </summary>
        public static async Task<string> ExportSalesCsvAsync(List<Order> orders)
        {
            Directory.CreateDirectory(DefaultExportFolder);
            string filename = $"sales_export_{DateTime.Now:yyyy-MM-dd_HHmmss}.csv";
            string path = Path.Combine(DefaultExportFolder, filename);

            var sb = new StringBuilder();
            sb.AppendLine("Order ID,Date,Time,Customer,Order Type,Payment Method,Cashier,Items,Subtotal,Total,Cash Tendered,Change Given");

            foreach (var o in orders.OrderByDescending(x => x.Timestamp))
            {
                string items = string.Join(" | ", o.Items.Select(i => $"{i.ProductName} x{i.Quantity}"));
                sb.AppendLine(string.Join(",",
                    Esc(o.OrderId),
                    o.Timestamp.ToString("yyyy-MM-dd"),
                    o.Timestamp.ToString("HH:mm:ss"),
                    Esc(o.CustomerName),
                    Esc(o.OrderType),
                    Esc(o.PaymentMethod),
                    Esc(o.CashierName),
                    Esc(items),
                    o.Subtotal.ToString("F2"),
                    o.Total.ToString("F2"),
                    o.CashTendered.ToString("F2"),
                    o.ChangeGiven.ToString("F2")
                ));
            }

            await File.WriteAllTextAsync(path, sb.ToString(), Encoding.UTF8);
            return path;
        }

        /// <summary>
        /// Exports all customers to a CSV file. Returns the saved file path.
        /// </summary>
        public static async Task<string> ExportCustomersCsvAsync(List<Customer> customers, List<Order> orders)
        {
            Directory.CreateDirectory(DefaultExportFolder);
            string filename = $"customers_export_{DateTime.Now:yyyy-MM-dd_HHmmss}.csv";
            string path = Path.Combine(DefaultExportFolder, filename);

            var sb = new StringBuilder();
            sb.AppendLine("Name,Email,Phone,Member Since,Status,Total Visits,Lifetime Value (₱),Notes");

            foreach (var c in customers.OrderBy(x => x.Name))
            {
                var custOrders = orders.Where(o => o.CustomerId == c.Id).ToList();
                int visits = custOrders.Count;
                decimal lifetime = custOrders.Sum(o => o.Total);

                sb.AppendLine(string.Join(",",
                    Esc(c.Name),
                    Esc(c.Email),
                    Esc(c.Phone),
                    c.MemberSince.ToString("yyyy-MM-dd"),
                    Esc(c.Status),
                    visits,
                    lifetime.ToString("F2"),
                    Esc(c.AdminNotes)
                ));
            }

            await File.WriteAllTextAsync(path, sb.ToString(), Encoding.UTF8);
            return path;
        }

        /// <summary>
        /// Exports all products to a CSV file. Returns the saved file path.
        /// </summary>
        public static async Task<string> ExportProductsCsvAsync(List<Product> products)
        {
            Directory.CreateDirectory(DefaultExportFolder);
            string filename = $"products_export_{DateTime.Now:yyyy-MM-dd_HHmmss}.csv";
            string path = Path.Combine(DefaultExportFolder, filename);

            var sb = new StringBuilder();
            sb.AppendLine("Product Name,Category,Price (₱),Total Sales,Created Date");

            foreach (var p in products.OrderBy(x => x.CategoryName).ThenBy(x => x.Name))
            {
                sb.AppendLine(string.Join(",",
                    Esc(p.Name),
                    Esc(p.CategoryName),
                    p.Price.ToString("F2"),
                    p.SalesCount,
                    p.CreatedAt.ToString("yyyy-MM-dd")
                ));
            }

            await File.WriteAllTextAsync(path, sb.ToString(), Encoding.UTF8);
            return path;
        }

        // ══════════════════════════════════════════════════════════════
        //  FULL BACKUP (ZIP)
        // ══════════════════════════════════════════════════════════════

        /// <summary>
        /// Creates a full local backup as a .zip archive containing all JSON data files.
        /// Returns the saved backup file path.
        /// </summary>
        public static async Task<string> CreateFullBackupAsync(string? customFolder = null)
        {
            string backupDir = customFolder ?? DefaultBackupFolder;
            Directory.CreateDirectory(backupDir);

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
            string zipPath = Path.Combine(backupDir, $"MatchaPOS_Backup_{timestamp}.zip");

            await Task.Run(() =>
            {
                // Create temp folder with copies of all data files
                string tempDir = Path.Combine(Path.GetTempPath(), $"MatchaPOS_Backup_{Guid.NewGuid():N}");
                Directory.CreateDirectory(tempDir);

                try
                {
                    // Copy all JSON data files
                    string[] dataFiles = { "settings.json", "products.json", "categories.json",
                                           "customers.json", "orders.json", "sessions.json" };

                    foreach (string file in dataFiles)
                    {
                        string src = Path.Combine(AppFolder, file);
                        if (File.Exists(src))
                        {
                            File.Copy(src, Path.Combine(tempDir, file), true);
                        }
                    }

                    // Copy Images folder if it exists
                    string imagesDir = Path.Combine(AppFolder, "Images");
                    if (Directory.Exists(imagesDir))
                    {
                        string destImagesDir = Path.Combine(tempDir, "Images");
                        Directory.CreateDirectory(destImagesDir);
                        foreach (var img in Directory.GetFiles(imagesDir))
                        {
                            File.Copy(img, Path.Combine(destImagesDir, Path.GetFileName(img)), true);
                        }
                    }

                    // Write backup metadata
                    string meta = $"Backup Created: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n" +
                                  $"Application: S.I.P.\n" +
                                  $"Version: 2.4.0-stable\n" +
                                  $"Machine: {Environment.MachineName}\n";
                    File.WriteAllText(Path.Combine(tempDir, "_backup_info.txt"), meta);

                    // Create ZIP (overwrite if exists)
                    if (File.Exists(zipPath)) File.Delete(zipPath);
                    ZipFile.CreateFromDirectory(tempDir, zipPath, CompressionLevel.Optimal, false);
                }
                finally
                {
                    // Clean up temp
                    try { Directory.Delete(tempDir, true); } catch { }
                }
            });

            return zipPath;
        }

        // ══════════════════════════════════════════════════════════════
        //  RESTORE BACKUP
        // ══════════════════════════════════════════════════════════════

        /// <summary>
        /// Validates that a ZIP file is a valid MatchaPOS backup.
        /// Returns a list of found data files.
        /// </summary>
        public static List<string> ValidateBackup(string zipPath)
        {
            var foundFiles = new List<string>();
            using var archive = ZipFile.OpenRead(zipPath);
            foreach (var entry in archive.Entries)
            {
                if (!string.IsNullOrEmpty(entry.Name))
                    foundFiles.Add(entry.FullName);
            }
            return foundFiles;
        }

        /// <summary>
        /// Restores all data from a backup ZIP archive. Overwrites current data.
        /// </summary>
        public static async Task RestoreBackupAsync(string zipPath)
        {
            await Task.Run(() =>
            {
                // Extract to temp first
                string tempDir = Path.Combine(Path.GetTempPath(), $"MatchaPOS_Restore_{Guid.NewGuid():N}");
                Directory.CreateDirectory(tempDir);

                try
                {
                    ZipFile.ExtractToDirectory(zipPath, tempDir, true);

                    // Copy JSON files back to AppFolder
                    string[] dataFiles = { "settings.json", "products.json", "categories.json",
                                           "customers.json", "orders.json", "sessions.json" };

                    foreach (string file in dataFiles)
                    {
                        string src = Path.Combine(tempDir, file);
                        if (File.Exists(src))
                        {
                            File.Copy(src, Path.Combine(AppFolder, file), true);
                        }
                    }

                    // Restore Images folder
                    string srcImages = Path.Combine(tempDir, "Images");
                    if (Directory.Exists(srcImages))
                    {
                        string destImages = Path.Combine(AppFolder, "Images");
                        Directory.CreateDirectory(destImages);
                        foreach (var img in Directory.GetFiles(srcImages))
                        {
                            File.Copy(img, Path.Combine(destImages, Path.GetFileName(img)), true);
                        }
                    }
                }
                finally
                {
                    try { Directory.Delete(tempDir, true); } catch { }
                }
            });
        }

        // ══════════════════════════════════════════════════════════════
        //  BACKUP STATUS INFO
        // ══════════════════════════════════════════════════════════════

        /// <summary>
        /// Gets the most recent backup file info from the default backup folder.
        /// </summary>
        public static (string? FilePath, DateTime? Date, long? SizeBytes) GetLastBackupInfo()
        {
            if (!Directory.Exists(DefaultBackupFolder))
                return (null, null, null);

            var files = Directory.GetFiles(DefaultBackupFolder, "MatchaPOS_Backup_*.zip")
                .OrderByDescending(f => File.GetCreationTime(f))
                .ToList();

            if (files.Count == 0)
                return (null, null, null);

            var latest = files[0];
            var info = new FileInfo(latest);
            return (latest, info.CreationTime, info.Length);
        }

        /// <summary>
        /// Gets total size of the data folder.
        /// </summary>
        public static long GetDatabaseSizeBytes()
        {
            if (!Directory.Exists(AppFolder)) return 0;

            long total = 0;
            string[] dataFiles = { "settings.json", "products.json", "categories.json",
                                   "customers.json", "orders.json", "sessions.json" };

            foreach (string file in dataFiles)
            {
                string path = Path.Combine(AppFolder, file);
                if (File.Exists(path))
                    total += new FileInfo(path).Length;
            }
            return total;
        }

        public static string GetDefaultBackupFolder() => DefaultBackupFolder;
        public static string GetDefaultExportFolder() => DefaultExportFolder;

        /// <summary>
        /// Formats a byte count into a human-readable string.
        /// </summary>
        public static string FormatSize(long bytes)
        {
            if (bytes < 1024) return $"{bytes} B";
            if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
            return $"{bytes / (1024.0 * 1024.0):F2} MB";
        }

        // ── Helpers ──────────────────────────────────────────────────
        private static string Esc(string? val)
        {
            if (string.IsNullOrEmpty(val)) return "\"\"";
            // Escape quotes and wrap in quotes for CSV safety
            return $"\"{val.Replace("\"", "\"\"")}\"";
        }
    }
}
