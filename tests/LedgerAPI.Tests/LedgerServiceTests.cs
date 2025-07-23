using Xunit;
using Moq;
using LedgerAPI.Services;
using LedgerAPI.Models;
using LedgerAPI.Repositories;
using System;
using System.Collections.Generic;

namespace LedgerAPI.Tests
{
public class LedgerServiceTests
{
    private readonly Mock<ILedgerRepository> _repositoryMock;
    private readonly LedgerService _ledgerService;

    public LedgerServiceTests()
    {
        _repositoryMock = new Mock<ILedgerRepository>();
        _ledgerService = new LedgerService(_repositoryMock.Object);
    }

        [Fact]
        public void AddTransaction_ShouldAddTransactionSuccessfully()
        {
            // Arrange
            var accountId = "account1";
            var transaction = new Transaction
            {
                Type = TransactionType.Deposit,
                Amount = 100,
                Description = "Test deposit"
            };

            // Act
            _ledgerService.AddTransaction(accountId, transaction);

            // Assert
            _repositoryMock.Verify(r => r.AddTransaction(accountId, transaction), Times.Once);
        }

        [Fact]
        public void GetBalance_ShouldReturnCorrectBalance()
        {
            // Arrange
            var accountId = "account1";
            var deposit = new Transaction
            {
                Type = TransactionType.Deposit,
                Amount = 100,
                Description = "Test deposit"
            };
            var withdrawal = new Transaction
            {
                Type = TransactionType.Withdrawal,
                Amount = 50,
                Description = "Test withdrawal"
            };

            _repositoryMock.Setup(r => r.GetBalance(accountId)).Returns(50);

            // Act
            var balance = _ledgerService.GetBalance(accountId);

            // Assert
            Assert.Equal(50, balance);
            _repositoryMock.Verify(r => r.GetBalance(accountId), Times.Once);
        }

        [Fact]
        public void GetTransactions_ShouldReturnAllTransactions()
        {
            // Arrange
            var accountId = "account1";
            var transaction1 = new Transaction
            {
                Type = TransactionType.Deposit,
                Amount = 100,
                Description = "Test deposit 1"
            };
            var transaction2 = new Transaction
            {
                Type = TransactionType.Deposit,
                Amount = 200,
                Description = "Test deposit 2"
            };

            var transactions = new List<Transaction> { transaction1, transaction2 };
            _repositoryMock.Setup(r => r.GetTransactions(accountId)).Returns(transactions);

            // Act
            var result = _ledgerService.GetTransactions(accountId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.Amount == 100);
            Assert.Contains(result, t => t.Amount == 200);
            _repositoryMock.Verify(r => r.GetTransactions(accountId), Times.Once);
        }

        [Fact]
        public void GetBalance_ShouldReturnZero_WhenNoTransactions()
        {
            // Arrange
            var accountId = "newAccount";
            _repositoryMock.Setup(r => r.GetBalance(accountId)).Returns(0);

            // Act
            var balance = _ledgerService.GetBalance(accountId);

            // Assert
            Assert.Equal(0, balance);
            _repositoryMock.Verify(r => r.GetBalance(accountId), Times.Once);
        }

        [Fact]
        public void GetAccountCount_ShouldReturnCorrectCount()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAccountCount()).Returns(2);

            // Act
            var count = _ledgerService.GetAccountCount();

            // Assert
            Assert.Equal(2, count);
            _repositoryMock.Verify(r => r.GetAccountCount(), Times.Once);
        }
    }
}
