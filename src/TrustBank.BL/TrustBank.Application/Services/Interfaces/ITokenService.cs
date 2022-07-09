using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrustBank.Application.Common;
using TrustBank.Core.Models;

namespace TrustBank.Application.Services.Interfaces
{
    public interface ITokenService<T> where T : class
    {
        string GenerateToken(Customer user, List<string> userRoles, IOptions<T> options);
        string BuildToken(string key, string issuer, string audience, Customer customer);
        bool ValidateToken(string key, string issuer, string audience, string token);
    }
}
