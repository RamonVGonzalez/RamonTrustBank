using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrustBank.Application.Dtos;
using TrustBank.Application.Responses;
using TrustBank.Application.Services.Interfaces;
using TrustBank.Core.Models;
using TrustBank.Core.Models.Authorization;
using TrustBank.Core.Models.Enums;
using TrustBank.Core.RepositoriesInterface;

namespace TrustBank.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        static Dictionary<string, string> AccountNumberStorage = new Dictionary<string, string>();

        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public CustomerService(
            ICustomerRepository customerRepository,
            IAccountRepository accountRepository,
            IProductRepository productRepository,
            UserManager<Customer> userManager,
            SignInManager<Customer> signInManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper
           )
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _mapper = mapper;
        }

        public async Task<Response<CustomerDisplayDto>> Authenticate(string email, string pin)
        {
            var response = new Response<CustomerDisplayDto>();

            var customer = await _userManager.FindByEmailAsync(email);
            if (customer != null)
            {

                if (VerifyPin(pin,customer.PinHash, customer.PinSalt))
                {
                    response.Message = "Authentication Successful";
                    response.Status = true;
                    response.Data = _mapper.Map<CustomerDisplayDto>(customer);
                }
            }

            return response;
        }

        public async Task<Response<CustomerDisplayDto>> CreateCustomer(Customer customer, string password, string pin)
        {
            var response = new Response<CustomerDisplayDto>();

            byte[] pinHash;
            byte[] pinSalt;

            CreatePinSaltAndPinHash(pin, out pinHash, out pinSalt);

            customer.DateCreated = DateTime.Now;
            customer.DateOfLastUpdate = DateTime.Now;
            customer.CustomerName = $"{customer.FirstName} {customer.LastName}";
            customer.UserName = customer.Email;
            customer.PinHash = pinHash;
            customer.PinSalt = pinSalt;


            var result = await _userManager.CreateAsync(customer, password);

            if (result.Succeeded)
            {
                response.Message = "Customer Added Successfully. ";


                if (EmailDomainIsTrustBank(customer.Email))
                {
                    var roleAdditionResult = await _userManager.AddToRoleAsync(customer, Role.Admin);
                    RoleAdditionCheck(response, roleAdditionResult);

                }
                else
                {
                    var roleAdditionResult = await _userManager.AddToRoleAsync(customer, Role.User);
                    RoleAdditionCheck(response, roleAdditionResult);
                }

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    response.Message = "Customer Not Added";
                    response.Errors.Add(error.Code, error.Description);
                }

            }
            
            response.Data = _mapper.Map<CustomerDisplayDto>(customer);

            return response;

        }

        public async Task<Response<AccountDisplayDto>> CreateAccount(Customer customer, Account account)
        {
            var response = new Response<AccountDisplayDto>();

            var accountNumber = string.Empty;

            var product = await _productRepository.GetByIdAsync(account.ProductId);

            do
            {
                accountNumber = GenerateAccountNumber(product.ProductType);

            } while (AccountNumberStorage.ContainsKey(accountNumber));

            account.AccountNumber = accountNumber;
            account.AccountStatus = AccountStatus.Active;
            account.ClosureStatus = ClosureStatus.N;
            account.LastTransactionDate = DateTime.Now;

            var result = await _accountRepository.AddAsync(account);

            if (result != null)
            {
                customer.Accounts.Add(account);
                response.Message = "Account Creation Successful";
                response.Status = true;
                response.Data = _mapper.Map<AccountDisplayDto>(account);
            }
            else
            {
                response.Message = "Account Creation Failed";
            }

            return response;
        }

        public async Task<Response<IEnumerable<AccountDisplayDto>>> GetAllAccountsAsync(string customerId)
        {
            var response = new Response<IEnumerable<AccountDisplayDto>>();

            IEnumerable<Account> accounts = await _customerRepository.GetAccountsByCustomerIdAsync(customerId);

            response.Message = "Success";
            response.Status = true;
            response.Data = _mapper.Map<IEnumerable<AccountDisplayDto>>(accounts);

            return response;
        }

        public async Task<Response<AccountDisplayDto>> GetAccountByAccountNumberAsync(string accountNumber)
        {
            var response = new Response<AccountDisplayDto>();

            var account = await _accountRepository.GetAccountByAccountNumberAsync(accountNumber);

            if (account != null)
            {
                response.Message = "Success";
                response.Status = true;
                response.Data = _mapper.Map<AccountDisplayDto>(account);
            }
            else
            {
                response.Message = $"Failed, no record found for {accountNumber}";
            }

            return response;
        }


        public async Task<Response<CustomerDisplayDto>> UpdateDetailsAsync(Customer customer, CustomerUpdateDto model)
        {
            var errorCounter = 0;

            var response = new Response<CustomerDisplayDto>();

            if (!(await _userManager.CheckPasswordAsync(customer, model.OldPassword)))
            {
                response.Message += "Password Validation Failed ";
                return response;

            }

            if (model.NewPassword != null)
            {
                if (model.OldPassword == model.NewPassword)
                {
                    response.Message += "Error, the new Password is the same as the old password. ";
                    errorCounter++;
                }
                else
                {
                    var result = await _userManager.ChangePasswordAsync(customer, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        response.Message += "Password Changed Successfully ";
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            response.Errors.Add(error.Code, error.Description);
                        }
                        errorCounter++;
                    }
                }
                      
            }

            if (model.NewPhoneNumber != null)
            {
                if (model.NewPhoneNumber == customer.PhoneNumber)
                {
                    response.Message += "Error, the new PhoneNumber is the same as the old PhoneNumber. ";
                    errorCounter++;
                }
                else
                {
                    var result = await _userManager.SetPhoneNumberAsync(customer, model.NewPhoneNumber);
                    
                    if (result.Succeeded)
                    {
                        response.Message += "PhoneNumber Changed Successfully ";
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            response.Errors.Add(error.Code, error.Description);
                        }
                        errorCounter++;
                    }
                }

            }

            if (model.NewEmail != null)
            {
                if (await _userManager.FindByEmailAsync(model.NewEmail) != null)
                {
                    if (model.NewEmail == customer.Email)
                    {               
                        response.Message += "Error, the new Email is the same as the old Email. ";
                    }
                    else
                    {
                        response.Message += "The Email Already Exists. ";
                    }

                    errorCounter++;
                }
                
                else
                {

                    var result = await _userManager.SetEmailAsync(customer, model.NewEmail);
                    if (result.Succeeded)
                    {
                        response.Message += "Email Changed Successfully ";
                        var usernameChange = await _userManager.SetUserNameAsync(customer,customer.Email);
                        if (usernameChange.Succeeded)
                        {
                            response.Message += "Username updated ";
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                response.Errors.Add(error.Code, error.Description);
                            }

                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            response.Errors.Add(error.Code, error.Description);
                        }
                    }
                }

            }

            var status = errorCounter > 0 ? false : true;
            return response;
        }

        public async Task<Response<CustomerDisplayDto>> CloseAccount(string accountNumberToClose, string adminEmail, string adminPin)
        {
            var response = new Response<CustomerDisplayDto>();

            var user = await _userManager.FindByEmailAsync(adminEmail);

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(Role.Admin))
                {
                    await _accountRepository.DeleteAsync(accountNumberToClose);
                    response.Message = "Account Closure Successful";
                    response.Status = true;
                }
                else
                {
                    response.Message = "Only Admins are Authorized to Close Accounts";
                }

            }
            else
            {
                response.Message = "Account closure failed, Admin does not exist";

            }

            return response;
        }

        private bool EmailDomainIsTrustBank(string email)
        {
            var emailDomain = email.Split("@")[1];
            if (emailDomain.Equals("TrustBank.com", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private void CreatePinSaltAndPinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }

        private static bool VerifyPin(string pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrWhiteSpace(pin)) return false;

            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (!(computedHash[i] == pinHash[i])) return false;
                }
            }
            return true;
        }

        private static void RoleAdditionCheck(Response<CustomerDisplayDto> response, IdentityResult roleAdditionResult)
        {
            if (roleAdditionResult.Succeeded)
            {
                response.Message += "Role addition successful";
                response.Status = true;
            }
            else
            {
                foreach (var error in roleAdditionResult.Errors)
                {
                    response.Message += "Role addition failed";
                    response.Errors.Add(error.Code, error.Description);
                }

            }
        }

        private string GenerateAccountNumber(ProductType accountType)
        {
            Random random = new Random();

            string accountNumber = "";

            switch (accountType)
            {
                case ProductType.Savings:
                    accountNumber = $"02{Math.Floor(random.NextDouble() * 10_000_000L + 20_000_000L)}";
                    break;
                case ProductType.Current:
                    accountNumber = $"01{Math.Floor(random.NextDouble() * 10_000_000L + 10_000_000L)}";
                    break;
                case ProductType.Internal:
                    accountNumber = $"71{Math.Floor(random.NextDouble() * 10_000_000L + 50_000_000L)}";
                    break;
                default:
                    accountNumber = $"03{Math.Floor(random.NextDouble() * 10_000_000L + 30_000_000L)}";
                    break;
            }

            return accountNumber;
        }


    }
}
