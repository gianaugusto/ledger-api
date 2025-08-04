using System;

namespace LedgerAPI.Models
{
    public enum TransactionType
    {
        Deposit = 0,
        Withdrawal = 1
    }

    public class Transaction
    {

        public required TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public required string Description { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class TransactionA : Transaction{

    }
}
