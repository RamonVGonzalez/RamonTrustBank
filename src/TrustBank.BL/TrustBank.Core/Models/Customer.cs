using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TrustBank.Core.Models
{
    public class Customer : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Address Address { get; set; }
        public List<Account> Accounts { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateOfLastUpdate { get; set; }
        public byte[] PinSalt { get; set; }
        public byte[] PinHash { get; set; }

        public Customer()
        {
            //CustomerName = $"{FirstName} {LastName}";
            //UserName = Email;
            Accounts = new List<Account>();
        }

    }
}
