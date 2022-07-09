using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrustBank.Core.Models;
using TrustBank.Core.Models.Enums;

namespace TrustBank.Core.RepositoriesInterface
{
    public interface ITransactionRepository
    {
        Task<Transaction> AddAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync(DateTime startDate, 
            DateTime endDate, TransactionStatus transactionStatus);
        Task<IEnumerable<Transaction>> GetAllTransactionsForAccountAsync(string accountNumber,
            DateTime startDate, DateTime endDate, TransactionStatus transactionStatus);
        Task<Transaction> GetTransactionByIdAsync(string id);
        
    }
}
