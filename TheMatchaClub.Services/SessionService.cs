using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheMatchaClubDomain.Models;

namespace TheMatchaClub.Services
{
    /// <summary>
    /// Manages the lifecycle of business sessions (store open → close).
    /// Provides methods for opening/closing sessions, linking orders,
    /// and querying session-based data for reports.
    /// </summary>
    public class SessionService
    {
        private readonly JsonDataService _data;

        public SessionService(JsonDataService data)
        {
            _data = data;
        }

        // ── Events ───────────────────────────────────────────────────
        public event EventHandler? SessionOpened;
        public event EventHandler? SessionClosed;

        // ── Active Session ───────────────────────────────────────────

        /// <summary>
        /// Returns the currently active (unclosed) session, or null.
        /// </summary>
        public BusinessSession? GetActiveSession()
        {
            return _data.Sessions.FirstOrDefault(s => !s.IsClosed);
        }

        /// <summary>
        /// Returns true if there is an active (unclosed) session.
        /// </summary>
        public bool HasActiveSession()
        {
            return GetActiveSession() != null;
        }

        // ── Open Session ─────────────────────────────────────────────

        /// <summary>
        /// Opens a new business session. Only one active session is allowed.
        /// </summary>
        /// <param name="cashierName">Name of the cashier/admin opening the store.</param>
        /// <param name="startingCash">Starting cash in the register drawer.</param>
        /// <returns>The newly created session.</returns>
        /// <exception cref="InvalidOperationException">If a session is already open.</exception>
        public async Task<BusinessSession> OpenSessionAsync(string cashierName, decimal startingCash)
        {
            if (HasActiveSession())
                throw new InvalidOperationException("A session is already active. Close it before opening a new one.");

            var session = new BusinessSession
            {
                OpenedAt = DateTime.Now,
                OpenedBy = cashierName,
                StartingCash = startingCash,
                IsClosed = false
            };

            _data.Sessions.Add(session);
            await _data.SaveSessionsAsync();

            SessionOpened?.Invoke(this, EventArgs.Empty);
            return session;
        }

        // ── Close Session ────────────────────────────────────────────

        /// <summary>
        /// Closes the active session, computing and freezing all totals.
        /// After closing, the session becomes immutable.
        /// </summary>
        /// <param name="actualCash">The actual cash counted in the register.</param>
        /// <param name="closedBy">Name of the person closing the session (optional, defaults to opener).</param>
        /// <returns>The closed session with computed totals.</returns>
        /// <exception cref="InvalidOperationException">If no active session exists.</exception>
        public async Task<BusinessSession> CloseSessionAsync(decimal actualCash, string? closedBy = null)
        {
            var session = GetActiveSession()
                ?? throw new InvalidOperationException("No active session to close.");

            // Validation: Actual cash cannot be negative
            if (actualCash < 0)
                throw new InvalidOperationException("Actual cash counted cannot be negative.");
            
            // Validation: Prevent 0 counted cash if there are recorded funds/sales
            if (actualCash == 0 && (session.StartingCash > 0 || GetSessionOrders(session.SessionId).Any()))
                throw new InvalidOperationException("Actual cash counted cannot be zero if there was a starting fund or sales recorded.");

            // Compute totals from linked orders
            ComputeSessionTotals(session);

            // Finalize
            session.ClosedAt = DateTime.Now;
            session.ClosedBy = closedBy ?? session.OpenedBy;
            session.ActualCash = actualCash;
            session.ExpectedCash = session.StartingCash + session.TotalRevenue;
            session.IsClosed = true;

            await _data.SaveSessionsAsync();

            SessionClosed?.Invoke(this, EventArgs.Empty);
            return session;
        }

        // ── Order Linkage ────────────────────────────────────────────

        /// <summary>
        /// Attaches an order to the currently active session.
        /// Call this before saving the order.
        /// If no active session exists, the order's SessionId remains null.
        /// </summary>
        public void AttachOrderToSession(Order order)
        {
            var session = GetActiveSession();
            if (session != null)
            {
                order.SessionId = session.SessionId;
            }
        }

        // ── Session Queries (for reporting) ──────────────────────────

        /// <summary>
        /// Returns all orders linked to a specific session.
        /// </summary>
        public List<Order> GetSessionOrders(Guid sessionId)
        {
            return _data.Orders.Where(o => o.SessionId == sessionId).ToList();
        }

        /// <summary>
        /// Returns all closed sessions, ordered by most recent first.
        /// </summary>
        public List<BusinessSession> GetClosedSessions()
        {
            return _data.Sessions
                .Where(s => s.IsClosed)
                .OrderByDescending(s => s.OpenedAt)
                .ToList();
        }

        /// <summary>
        /// Returns all dates that have at least one session (for calendar filtering).
        /// </summary>
        public List<DateTime> GetSessionDates()
        {
            return _data.Sessions
                .Select(s => s.OpenedAt.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();
        }

        /// <summary>
        /// Returns a session by its ID.
        /// </summary>
        public BusinessSession? GetSession(Guid sessionId)
        {
            return _data.Sessions.FirstOrDefault(s => s.SessionId == sessionId);
        }

        // ── Internal: Compute Totals ─────────────────────────────────

        /// <summary>
        /// Recalculates session totals from linked orders. Called during close.
        /// </summary>
        public void ComputeSessionTotals(BusinessSession session)
        {
            var orders = GetSessionOrders(session.SessionId);
            session.TotalRevenue = orders.Sum(o => o.Total);
            session.TotalTransactions = orders.Count;
            session.TotalUnitsSold = orders.SelectMany(o => o.Items).Sum(i => i.Quantity);
        }

        // ── Analytics Queries (centralized for reports) ──────────────

        /// <summary>
        /// Returns revenue grouped by hour of day for a session.
        /// </summary>
        public Dictionary<int, decimal> GetHourlySalesData(Guid sessionId)
        {
            var orders = GetSessionOrders(sessionId);
            var result = new Dictionary<int, decimal>();
            for (int h = 0; h < 24; h++) result[h] = 0;
            foreach (var o in orders)
                result[o.Timestamp.Hour] += o.Total;
            return result;
        }

        /// <summary>
        /// Returns revenue grouped by product category for a session.
        /// </summary>
        public Dictionary<string, decimal> GetCategorySalesData(Guid sessionId)
        {
            return GetSessionOrders(sessionId)
                .SelectMany(o => o.Items)
                .GroupBy(i => string.IsNullOrEmpty(i.CategoryName) ? "Other" : i.CategoryName)
                .ToDictionary(g => g.Key, g => g.Sum(i => i.LineTotal));
        }

        /// <summary>
        /// Returns top N items by units sold for a session.
        /// </summary>
        public List<(string Name, string Category, int Units, decimal Revenue)> GetTopItems(Guid sessionId, int count = 5)
        {
            return GetSessionOrders(sessionId)
                .SelectMany(o => o.Items)
                .GroupBy(i => new { i.ProductName, i.CategoryName })
                .Select(g => (g.Key.ProductName, g.Key.CategoryName, g.Sum(i => i.Quantity), g.Sum(i => i.LineTotal)))
                .OrderByDescending(x => x.Item3)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Returns all product sales for a session (for Sales Summary).
        /// </summary>
        public List<(string Name, string Category, int Units, decimal Revenue)> GetAllItemSales(Guid sessionId)
        {
            return GetSessionOrders(sessionId)
                .SelectMany(o => o.Items)
                .GroupBy(i => new { i.ProductName, i.CategoryName })
                .Select(g => (g.Key.ProductName, g.Key.CategoryName, g.Sum(i => i.Quantity), g.Sum(i => i.LineTotal)))
                .OrderByDescending(x => x.Item4)
                .ToList();
        }

        /// <summary>
        /// Returns the session that opened on a specific date.
        /// </summary>
        public BusinessSession? GetSessionByDate(DateTime date)
        {
            return _data.Sessions.FirstOrDefault(s => s.OpenedAt.Date == date.Date);
        }

        /// <summary>
        /// Returns the most recent session (active first, then latest closed).
        /// </summary>
        public BusinessSession? GetLatestSession()
        {
            return GetActiveSession()
                ?? _data.Sessions.OrderByDescending(s => s.OpenedAt).FirstOrDefault();
        }

        /// <summary>
        /// Returns all sessions ordered by date descending.
        /// </summary>
        public List<BusinessSession> GetAllSessions()
        {
            return _data.Sessions.OrderByDescending(s => s.OpenedAt).ToList();
        }
    }
}
