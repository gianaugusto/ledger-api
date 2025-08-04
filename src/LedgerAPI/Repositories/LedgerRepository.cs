using LedgerAPI.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LedgerAPI.Repositories
{
    public class LedgerRepository : ILedgerRepository
    {
        private readonly ConcurrentDictionary<string, List<Transaction>> _accounts =
            new();

        public void AddTransaction(string accountId, Transaction transaction)
        {
            transaction.Timestamp=DateTime.UtcNow;
            var transactions = _accounts.GetOrAdd(accountId, _ => []);
            transactions.Add(transaction);
        }

        public decimal GetBalance(string accountId, DateTime statDate, DateTime endDate)
        {
            if (_accounts.TryGetValue(accountId, out var transactions))
            {
                return transactions.Where(o => o.Timestamp >= statDate && o.Timestamp <= endDate).Sum(t => t.Type == TransactionType.Deposit ? t.Amount : -t.Amount);
            }

            return 0;
        }

         public decimal GetBalance(string accountId)
        {
            if (_accounts.TryGetValue(accountId, out var transactions))
            {
                return transactions.Sum(t => t.Type == TransactionType.Deposit ? t.Amount : -t.Amount);
            }

            return 0;
        }

        public List<Transaction> GetTransactions(string accountId)
        {
            if (_accounts.TryGetValue(accountId, out var transactions))
            {
                return transactions;
            }
            return [];
        }

        public int GetAccountCount()
        {
            return _accounts.Count;
        }
    }
}
