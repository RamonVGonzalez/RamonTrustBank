using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TrustBank.Application.Common;
using TrustBank.Application.Responses;
using TrustBank.Application.Services.Interfaces;
using TrustBank.Core.Models;
using TrustBank.Core.Models.Enums;
using TrustBank.Core.RepositoriesInterface;
using TrustBank.Infrastructure.Data;

namespace TrustBank.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<Customer> _userManager;
        private readonly IOptions<BankSettings> appSettingsOptions;
        private readonly IProductRepository _productRepository;
        private readonly BankSettings _appSettings;

        public TransactionService(ITransactionRepository transactionRepository, ApplicationDbContext dbContext, ICustomerRepository customerRepository, IAccountRepository accountRepository,
            UserManager<Customer> userManager, IOptions<BankSettings> appSettingsOptions, IProductRepository productRepository)
        {
            _transactionRepository = transactionRepository;
            _dbContext = dbContext;
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _userManager = userManager;
            this.appSettingsOptions = appSettingsOptions;
            _productRepository = productRepository;
            _appSettings = appSettingsOptions.Value;
        }


        public async Task<Response<List<Transaction>>> FindTransactionsBydate(DateTime startDate, DateTime endDate,
            TransactionStatus transactionStatus = TransactionStatus.Success, int pageNumber = 1, int recordsPerPage = 10)
        {
            var response = new Response<List<Transaction>>();

            var transactions = await _transactionRepository.GetAllTransactionsAsync(startDate, endDate, transactionStatus);

            response.Status = true;
            response.Data = transactions.Skip(recordsPerPage * (pageNumber - 1)).Take(recordsPerPage).ToList();

            return response;
        }

        public async Task<Response<Transaction>> MakeDeposit(string accountNumber, decimal amount, string transactionPin)
        {
            var response = new Response<Transaction>();

            if (amount <= 0)
            {
                response.Message = "Please enter a valid withdrawal amount";
                return response;
            }

            var account = await _accountRepository.GetAccountByAccountNumberAsync(accountNumber);

            if (account == null)
            {
                response.Message = "Account does not exist";
                return response;
            }

            var settlementAccount = await _accountRepository.GetAccountByAccountNumberAsync(_appSettings.SettlementAccountNumber);

            var transaction = new Transaction
            {
                Amount = amount,
                DebitAccount = settlementAccount.AccountNumber,
                CreditAccount = account.AccountNumber,
                Id = Guid.NewGuid().ToString(),
                TransactionType = TransactionType.Deposit,
                DateCreated = DateTime.Now,
                DateOfLastUpdate = DateTime.Now,
                TransactionStatus = TransactionStatus.Pending

            };
    

            if (settlementAccount.AccountBalance < amount)
            {
                response.Message = "Please hold on for Cash Loading";
                transaction.TransactionStatus = TransactionStatus.Failed;
                await _transactionRepository.AddAsync(transaction);
                return response;
            }


            settlementAccount.AccountBalance -= amount;
            account.AccountBalance += amount;
            
            TransactionStatusCheck(response, account, settlementAccount, transaction);

            account.LastTransactionDate = DateTime.Now;

            if (account.AccountStatus == AccountStatus.Inactive)
            {
                account.AccountStatus = AccountStatus.Active;
                account.ReactivationDate = DateTime.Now;
                response.Message += "Reactivation Successful";
            }

            transaction.TransactionReference = $"Transaction From Source => {JsonSerializer.Serialize(transaction.DebitAccount)} to destination account => " +
            $"{JsonSerializer.Serialize(transaction.CreditAccount)} on Date => {transaction.DateCreated} for amount => {JsonSerializer.Serialize(transaction.Amount)} " +
            $"Transaction Type =>{transaction.TransactionType} Transaction Status => {transaction.TransactionStatus}";

            await _transactionRepository.AddAsync(transaction);

            response.Data = transaction;

            return response;
        }

        
        public async Task<Response<Transaction>> MakeWithdrawal(string accountNumber, decimal amount, string transactionPin)
        {
            var response = new Response<Transaction>();

            if (amount <= 0)
            {
                response.Message = "Please enter a valid withdrawal amount";
                return response;
            }

            var account = await _accountRepository.GetAccountByAccountNumberAsync(accountNumber);

            if (account == null)
            {
                response.Message = "Account does not exist";
                return response;
            }

            if (!(account.AccountStatus == AccountStatus.Active))
            {
                response.Message = "Please reactivate account to make withdrawal";
                return response;
            }

            var minimumBalanceOfProduct = (await _productRepository.GetByIdAsync(account.ProductId)).MinimumBalanceForProduct;

            if (amount > account.AccountBalance - minimumBalanceOfProduct)
            {
                response.Message = $"Insufficient Balance: Available Balance => {account.AccountBalance - minimumBalanceOfProduct}";
                return response;
            }

            var settlementAccount = await _accountRepository.GetAccountByAccountNumberAsync(_appSettings.SettlementAccountNumber);

            var transaction = new Transaction
            {
                Amount = amount,
                DebitAccount = account.AccountNumber,
                CreditAccount = settlementAccount.AccountNumber,
                Id = Guid.NewGuid().ToString(),
                TransactionType = TransactionType.Withdrawal,
                DateCreated = DateTime.Now,
                DateOfLastUpdate = DateTime.Now,
                TransactionStatus = TransactionStatus.Pending

            };

            account.AccountBalance -= amount;
            settlementAccount.AccountBalance += amount;

            TransactionStatusCheck(response, account, settlementAccount, transaction);

            account.LastTransactionDate = DateTime.Now;

            transaction.TransactionReference = $"Transaction From Source => {JsonSerializer.Serialize(transaction.DebitAccount)} to destination account => " +
            $"{JsonSerializer.Serialize(transaction.CreditAccount)} on Date => {transaction.DateCreated} for amount => {JsonSerializer.Serialize(transaction.Amount)} " +
            $"Transaction Type =>{transaction.TransactionType} Transaction Status => {transaction.TransactionStatus}";

            await _transactionRepository.AddAsync(transaction);

            response.Data = transaction;

            return response;
        }

        public async Task<Response<Transaction>> MakeFundsTransfer(string fromAccount, string toAccount, decimal amount, string transactionPin, string narration)
        {
            var response = new Response<Transaction>();

            if (amount <= 0)
            {
                response.Message = $"Please enter an amount greater than zero value entered : {amount}";
                return response;

            }

            var debitAccount = await _accountRepository.GetAccountByAccountNumberAsync(fromAccount);
            
            if (debitAccount == null)
            {
                response.Message = $"Account: {fromAccount} does not exist";
                return response;
            }

            if (!(debitAccount.AccountStatus == AccountStatus.Active))
            {
                response.Message = $"Please reactivate account: {debitAccount} to make withdrawal";
                return response;
            }


            var minimumBalance = (await _productRepository.GetByIdAsync(debitAccount.ProductId)).MinimumBalanceForProduct;

            if (amount > debitAccount.AccountBalance - minimumBalance)
            {
                response.Message = $"Insufficient Balance: Available Balance => {debitAccount.AccountBalance - minimumBalance}";
                return response;
            }


            var creditAccountValidationResult = await ValidateAccount(toAccount);

            if (!creditAccountValidationResult.Status)
            {
                response.Message = creditAccountValidationResult.Message;
                return response;
            }

            var creditAccountName = creditAccountValidationResult.Message;
            var creditAccount = creditAccountValidationResult.Data;

            var transaction = new Transaction()
            {
                Id = Guid.NewGuid().ToString(),
                Amount = amount,
                CreditAccount = toAccount,
                DebitAccount = fromAccount,
                DateCreated = DateTime.Now,
                DateOfLastUpdate = DateTime.Now,
                Narration = narration,
                TransactionType = TransactionType.FundsTransfer,
                TransactionStatus = TransactionStatus.Pending
            };


            debitAccount.AccountBalance -= amount;
            creditAccount.AccountBalance += amount;

            TransactionStatusCheck(response, creditAccount, debitAccount, transaction);


            debitAccount.LastTransactionDate = DateTime.Now;
            creditAccount.LastTransactionDate = DateTime.Now;

            if (creditAccount.AccountStatus == AccountStatus.Inactive)
            {
                creditAccount.AccountStatus = AccountStatus.Active;
                creditAccount.ReactivationDate = DateTime.Now;
                response.Message += "Reactivation Successful";
            }


            transaction.TransactionReference = $"Transaction From Source => {JsonSerializer.Serialize(transaction.DebitAccount)} to destination account => " +
            $"{JsonSerializer.Serialize(transaction.CreditAccount)} on Date => {transaction.DateCreated} for amount => {JsonSerializer.Serialize(transaction.Amount)} " +
            $"Transaction Type =>{transaction.TransactionType} Transaction Status => {transaction.TransactionStatus}";

            await _transactionRepository.AddAsync(transaction);

            response.Data = transaction;

            return response;          


        }

        private void TransactionStatusCheck(Response<Transaction> response, Account creditAccount, Account debitAccount, Transaction transaction)
        {
            if (_dbContext.Entry(creditAccount).State == EntityState.Modified 
                && _dbContext.Entry(debitAccount).State == EntityState.Modified)
            {
                transaction.TransactionStatus = TransactionStatus.Success;
                response.Status = true;
                response.Message = "Transaction Successful";
            }
            else
            {
                response.Message = "An error occcurred";
                transaction.TransactionStatus = TransactionStatus.Failed;
            }
        }

        private async Task<Response<Account>> ValidateAccount(string accountNumber)
        {
            var response = new Response<Account>();

            var account = await _accountRepository.GetAccountByAccountNumberAsync(accountNumber);

            if (account == null)
            {
                response.Message = $"Account Validation Failed, {accountNumber} not found";
                return response;
            }

            response.Status = true;
            response.Message = account.AccountName;
            response.Data = account;
            return response;
        }
    }
}
