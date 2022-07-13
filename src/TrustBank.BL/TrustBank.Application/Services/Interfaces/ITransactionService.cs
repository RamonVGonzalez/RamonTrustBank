using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrustBank.Application.Responses;
using TrustBank.Core.Models;
using TrustBank.Core.Models.Enums;

namespace TrustBank.Application.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Response<List<Transaction>>> FindTransactionsBydate(DateTime startDate, DateTime endDate, TransactionStatus transactionStatus, int pageNumber, int recordsPerPage);
        Task<Response<Transaction>> MakeDeposit(String accountNumber, decimal amount, string transactionPin);
        Task<Response<Transaction>> MakeWithdrawal(String accountNumber, decimal amount, string transactionPin);
        Task<Response<Transaction>> MakeFundsTransfer(string fromAccount, string toAccount, decimal amount, string transactionPin, string narration);

    }
}
