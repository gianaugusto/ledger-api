using LedgerAPI.Models;
using LedgerAPI.Repositories;
using System.Collections.Generic;

namespace LedgerAPI.Services
{
    public class LedgerService
    {
        private readonly ILedgerRepository _repository;

        public LedgerService(ILedgerRepository repository)
        {
            _repository = repository;
        }

        public void AddTransaction(string accountId, Transaction transaction)
        {
            _repository.AddTransaction(accountId, transaction);
        }

        public decimal GetBalance(string accountId)
        {
            return _repository.GetBalance(accountId);
        }

        public List<Transaction> GetTransactions(string accountId)
        {
            return _repository.GetTransactions(accountId);
        }

        public int GetAccountCount()
        {
            return _repository.GetAccountCount();
        }
    }
}
