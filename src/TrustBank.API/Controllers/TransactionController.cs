using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrustBank.Application.Dtos;
using TrustBank.Application.Services.Interfaces;

namespace TrustBank.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetTransactions(TransactionRecordsRequestModel model)
        {
           return Ok(await _transactionService.FindTransactionsBydate(model.StartDate, model.EndDate, model.TransactionStatus, model.pageNumber, model.RecordsPerPage));

        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Deposit(DepositAndWithdrawalRequestModel model)
        {
            return Ok( await _transactionService.MakeDeposit(model.AccountNumber,model.Amount, model.Pin));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Withdrawal(DepositAndWithdrawalRequestModel model)
        {
            return Ok(await _transactionService.MakeWithdrawal(model.AccountNumber, model.Amount, model.Pin));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> FundsTransfer(FundsTransferRequestModel model)
        {
            return Ok(await _transactionService.MakeFundsTransfer(model.FromAccount, model.ToAccount, model.Amount, model.Pin, model.Narration));
        }

    }
}
