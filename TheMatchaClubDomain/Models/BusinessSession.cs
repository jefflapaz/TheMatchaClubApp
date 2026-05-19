using System;

namespace TheMatchaClubDomain.Models
{
    /// <summary>
    /// Represents a single business operating session (store open → close cycle).
    /// All orders created during this session are linked via SessionId.
    /// Once closed, the session becomes immutable/read-only.
    /// </summary>
    public class BusinessSession
    {
        public Guid SessionId { get; set; } = Guid.NewGuid();

        // ── Timestamps ───────────────────────────────────────────────
        public DateTime OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        // ── Personnel ────────────────────────────────────────────────
        public string OpenedBy { get; set; } = string.Empty;
        public string? ClosedBy { get; set; }

        // ── Cash Management ──────────────────────────────────────────
        public decimal StartingCash { get; set; }
        public decimal ExpectedCash { get; set; }
        public decimal ActualCash { get; set; }

        // ── Computed Totals (frozen on close) ─────────────────────────
        public decimal TotalRevenue { get; set; }
        public int TotalTransactions { get; set; }
        public int TotalUnitsSold { get; set; }

        // ── Status ───────────────────────────────────────────────────
        public bool IsClosed { get; set; }
    }
}
