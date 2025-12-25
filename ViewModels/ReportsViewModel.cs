using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;  // FIXED: Add this for [Column] attribute
using RealtorsPortal.Models;

namespace RealtorsPortal.ViewModels
{
    public class ReportsViewModel
    {
        public StatsModel Stats { get; set; } = new StatsModel();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

    public class StatsModel
    {
        public int PaymentsToday { get; set; } = 0;
        public int UnapprovedProperties { get; set; } = 0;
        public int TotalCategories { get; set; } = 0;
        public int TotalPrivateSellers { get; set; } = 0;
        public int TotalAgents { get; set; } = 0;
    }

    public class Transaction
    {
        public DateTime Date { get; set; } = DateTime.Now;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } = 0;
        public string MemberEmail { get; set; } = "";
        public string Details { get; set; } = "";
    }
}