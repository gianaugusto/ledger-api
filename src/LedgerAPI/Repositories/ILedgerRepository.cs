using LedgerAPI.Models;
using System.Collections.Generic;

namespace LedgerAPI.Repositories
{
    public interface ILedgerRepository
    {
        void AddTransaction(string accountId, Transaction transaction);
        decimal GetBalance(string accountId);
        List<Transaction> GetTransactions(string accountId);
        int GetAccountCount();
    }
}
