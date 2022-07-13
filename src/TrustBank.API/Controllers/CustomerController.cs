using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrustBank.Application.Dtos;
using TrustBank.Application.Services.Interfaces;
using TrustBank.Core.Models;

namespace TrustBank.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly UserManager<Customer> _userManager;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerController(UserManager<Customer> userManager, ICustomerService customerService, IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [HttpPost]
        [Route("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto model)
        {

            var customer = _mapper.Map<Customer>(model);

            return Ok(await _customerService.CreateCustomer(customer, model.Password, model.Pin));
        }


        [HttpPost]
        [Route("CreateAccount")]

        public async Task<IActionResult> CreateAccount(CreateAccountDto model)
        {
            var customer = await _userManager.FindByNameAsync(User.Identity.Name);
            var response = await _customerService.Authenticate(customer.Email, model.Pin);

            if (response.Status)
            {
                var account = _mapper.Map<Account>(model);
                _mapper.Map(customer, account);

                return Ok(await _customerService.CreateAccount(customer, account));
            }
            return Forbid("Pin Validation Failed");

        }

        [HttpGet]
        [Route("GetAccounts")]
        public async Task<IActionResult> GetAccounts()
        {
            var userName = User.Identity.Name;
            var customer = await _userManager.FindByNameAsync(userName);
            return Ok(await _customerService.GetAllAccountsAsync(customer.Id));
        }


        [HttpGet]
        [Route("GetAccount")]
        public async Task<IActionResult> GetAccount(string accountNumber)
        {
            return Ok(await _customerService.GetAccountByAccountNumberAsync(accountNumber));

        }


        [HttpPatch]
        [Route("UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer(CustomerUpdateDto model)
        {
            var userName = User.Identity.Name;
            var customer = await _userManager.FindByNameAsync(userName);
            return Ok(await _customerService.UpdateDetailsAsync(customer,model));
        }

        [HttpPost]
        [Route("CloseAccount")]
        public async Task<IActionResult> CloseAccount([FromBody] CloseAccountDto model)
        {
            return Ok(await _customerService.CloseAccount(model.AccountNumberToClose, model.AdminEmail, model.AdminPin));
        }
    }
}
