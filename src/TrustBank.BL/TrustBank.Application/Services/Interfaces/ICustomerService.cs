using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustBank.Application.Dtos;
using TrustBank.Application.Responses;
using TrustBank.Core.Models;

namespace TrustBank.Application.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<Response<CustomerDisplayDto>> CreateCustomer(Customer customer, string password, string pin);
        Task<Response<CustomerDisplayDto>> Authenticate(string email, string pin);
        Task<Response<IEnumerable<AccountDisplayDto>>> GetAllAccountsAsync(string customerId);
        Task<Response<AccountDisplayDto>> GetAccountByAccountNumberAsync(string accountNumber);

        Task<Response<CustomerDisplayDto>> UpdateDetailsAsync(Customer customer, CustomerUpdateDto customerUpdateDto);

        Task<Response<AccountDisplayDto>> CreateAccount(Customer customer, Account account);

        Task<Response<CustomerDisplayDto>> CloseAccount(string accountNumberToClose, 
            string adminEmail,  string adminPin);
    }
}
