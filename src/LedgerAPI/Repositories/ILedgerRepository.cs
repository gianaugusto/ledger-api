using LedgerAPI.Models;
using System.Collections.Generic;
using System;

namespace LedgerAPI.Repositories
{
    public interface ILedgerRepository
    {
        void AddTransaction(string accountId, Transaction transaction);
        decimal GetBalance(string accountId);
        decimal GetBalance(string accountId, DateTime statDate, DateTime endDate);
        List<Transaction> GetTransactions(string accountId);
        int GetAccountCount();
    }
}
