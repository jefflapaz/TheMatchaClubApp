using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClubApp.Core.Services
{
    public class JsonDataService
    {
        private static readonly string AppFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TheMatchaClub");

        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        // ── Data Stores ──────────────────────────────────────────────
        public StoreSettings Settings { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public List<Customer> Customers { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
        public List<BusinessSession> Sessions { get; set; } = new();

        // ── Events ───────────────────────────────────────────────────
        public event EventHandler? ProductsChanged;
        public event EventHandler? CategoriesChanged;
        public event EventHandler? OrdersChanged;
        public event EventHandler? CustomersChanged;
        public event EventHandler? SettingsChanged;
        public event EventHandler? SessionsChanged;
        public event EventHandler? DataLoaded;

        // ── Constructor ──────────────────────────────────────────────
        public JsonDataService()
        {
            Directory.CreateDirectory(AppFolder);
            Directory.CreateDirectory(Path.Combine(AppFolder, "Images"));
        }

        // ── Load All ─────────────────────────────────────────────────
        public async Task LoadAllAsync()
        {
            Settings   = await LoadAsync<StoreSettings>("settings.json") ?? new StoreSettings();
            Products   = await LoadAsync<List<Product>>("products.json") ?? new List<Product>();
            Categories = await LoadAsync<List<string>>("categories.json") ?? new List<string>();
            Customers  = await LoadAsync<List<Customer>>("customers.json") ?? new List<Customer>();
            Orders     = await LoadAsync<List<Order>>("orders.json") ?? new List<Order>();
            Sessions   = await LoadAsync<List<BusinessSession>>("sessions.json") ?? new List<BusinessSession>();

            DataLoaded?.Invoke(this, EventArgs.Empty);
        }

        // ── Save Methods (Task.Run to prevent UI stutter) ────────────
        public async Task SaveSettingsAsync()
        {
            await SaveAsync("settings.json", Settings);
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task SaveProductsAsync()
        {
            await SaveAsync("products.json", Products);
            ProductsChanged?.Invoke(this, EventArgs.Empty);

            // Auto-delete empty categories
            bool changed = false;
            for (int i = Categories.Count - 1; i >= 0; i--)
            {
                var cat = Categories[i];
                if (cat == "All Items") continue; // Protected
                
                if (!Products.Any(p => string.Equals(p.CategoryName, cat, StringComparison.OrdinalIgnoreCase)))
                {
                    Categories.RemoveAt(i);
                    changed = true;
                }
            }
            if (changed)
            {
                await SaveCategoriesAsync();
            }
        }

        public async Task SaveCategoriesAsync()
        {
            await SaveAsync("categories.json", Categories);
            CategoriesChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task SaveCustomersAsync()
        {
            await SaveAsync("customers.json", Customers);
            CustomersChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task SaveOrdersAsync()
        {
            await SaveAsync("orders.json", Orders);
            OrdersChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task SaveSessionsAsync()
        {
            await SaveAsync("sessions.json", Sessions);
            SessionsChanged?.Invoke(this, EventArgs.Empty);
        }

        // ── Core I/O (wrapped in Task.Run) ───────────────────────────
        public string GenerateOrderNumber()
        {
            string datePart = DateTime.Now.ToString("yyyyMMdd");
            string prefix = $"ORD-{datePart}-";
            
            // Find the highest sequence number for today
            int lastSeq = 0;
            var todayOrders = Orders.Where(o => o.OrderId.StartsWith(prefix)).ToList();
            if (todayOrders.Any())
            {
                var seqs = todayOrders
                    .Select(o => o.OrderId.Replace(prefix, ""))
                    .Select(s => int.TryParse(s, out int n) ? n : 0);
                lastSeq = seqs.Max();
            }
            
            return $"{prefix}{(lastSeq + 1):D4}";
        }

        private async Task<T?> LoadAsync<T>(string filename)
        {
            var path = Path.Combine(AppFolder, filename);
            if (!File.Exists(path)) return default;
            return await Task.Run(() =>
            {
                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<T>(json, JsonOpts);
            });
        }

        private async Task SaveAsync<T>(string filename, T data)
        {
            var path = Path.Combine(AppFolder, filename);
            await Task.Run(() =>
            {
                var json = JsonSerializer.Serialize(data, JsonOpts);
                File.WriteAllText(path, json);
            });
        }

        // ── Image Helper (copy to local Images folder) ───────────────
        public string CopyImageToLocal(string sourcePath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || !File.Exists(sourcePath))
                return string.Empty;

            // If it's already in our images folder, just return the name
            var imagesDir = GetImagesFolder();
            if (sourcePath.StartsWith(imagesDir, StringComparison.OrdinalIgnoreCase))
                return Path.GetFileName(sourcePath);

            var destName = $"{Guid.NewGuid()}{Path.GetExtension(sourcePath)}";
            var destPath = Path.Combine(imagesDir, destName);
            File.Copy(sourcePath, destPath, true);
            return destName;
        }

        public string GetFullImagePath(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath)) return string.Empty;
            
            // If it's already an absolute path that exists, use it
            if (Path.IsPathRooted(imagePath) && File.Exists(imagePath))
                return imagePath;

            // Otherwise, assume it's a filename in our images folder
            return Path.Combine(GetImagesFolder(), imagePath);
        }

        public string GetImagesFolder() => Path.Combine(AppFolder, "Images");
    }
}
