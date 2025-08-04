using Xunit;
using Moq;
using LedgerAPI.Services;
using LedgerAPI.Models;
using LedgerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

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
    public void GetBalance_ShouldReturnCorrectBalance_WithDateRange()
    {
        // Arrange
        var accountId = "account1";
        var startDate = DateTime.UtcNow.AddDays(-7);
        var endDate = DateTime.UtcNow;

        var transaction1 = new Transaction
        {
            Type = TransactionType.Deposit,
            Amount = 100,
            Description = "Test deposit 1",
            Timestamp = startDate.AddDays(1)
        };
        var transaction2 = new Transaction
        {
            Type = TransactionType.Deposit,
            Amount = 200,
            Description = "Test deposit 2",
            Timestamp = startDate.AddDays(3)
        };
        var transaction3 = new Transaction
        {
            Type = TransactionType.Withdrawal,
            Amount = 50,
            Description = "Test withdrawal",
            Timestamp = startDate.AddDays(-1) // Outside the date range
        };

        var transactions = new List<Transaction> { transaction1, transaction2, transaction3 };
        _repositoryMock.Setup(r => r.GetBalance(accountId, startDate, endDate)).Returns(transactions
            .Where(t => t.Timestamp >= startDate && t.Timestamp <= endDate)
            .Sum(t => t.Type == TransactionType.Deposit ? t.Amount : -t.Amount));

        // Act
        var balance = _ledgerService.GetBalance(accountId, startDate, endDate);

        // Assert
        Assert.Equal(300, balance); // 100 + 200 (transaction3 is outside the date range)
        _repositoryMock.Verify(r => r.GetBalance(accountId, startDate, endDate), Times.Once);
    }

    [Fact]
    public void GetBalance_ShouldReturnZero_WithDateRange_WhenNoTransactionsInRange()
    {
        // Arrange
        var accountId = "account1";
        var startDate = DateTime.UtcNow.AddDays(-7);
        var endDate = DateTime.UtcNow;

        var transaction1 = new Transaction
        {
            Type = TransactionType.Deposit,
            Amount = 100,
            Description = "Test deposit 1",
            Timestamp = startDate.AddDays(-1) // Outside the date range
        };

        var transactions = new List<Transaction> { transaction1 };
        _repositoryMock.Setup(r => r.GetBalance(accountId, startDate, endDate)).Returns(transactions
            .Where(t => t.Timestamp >= startDate && t.Timestamp <= endDate)
            .Sum(t => t.Type == TransactionType.Deposit ? t.Amount : -t.Amount));

        // Act
        var balance = _ledgerService.GetBalance(accountId, startDate, endDate);

        // Assert
        Assert.Equal(0, balance);
        _repositoryMock.Verify(r => r.GetBalance(accountId, startDate, endDate), Times.Once);
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
