using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrustBank.Core.Models;
using TrustBank.Core.Models.Enums;
using TrustBank.Core.RepositoriesInterface;
using TrustBank.Infrastructure.Data;

namespace TrustBank.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);

            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync(DateTime startDate, DateTime endDate, TransactionStatus transactionStatus)
        {

            var transactions = await _context.Transactions.Where(x =>
               (EF.Functions.DateDiffDay(startDate, x.DateCreated) >= 0) &&
               (EF.Functions.DateDiffDay(x.DateCreated, endDate) >= 0) &&
               (x.TransactionStatus == transactionStatus)
               ).ToListAsync();

            return transactions;

        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsForAccountAsync(string accountNumber, DateTime startDate, DateTime endDate, TransactionStatus transactionStatus)
        {
            var transactions = await _context.Transactions.Where(x => x.CreditAccount == accountNumber
            || x.DebitAccount == accountNumber)
                .ToListAsync();

            return transactions;
        }

        public async Task<Transaction> GetTransactionByIdAsync(string id)
        {
            return await _context.Transactions.FindAsync(id);
            //not sure FindAsync would work with a string

        }
    }
}
