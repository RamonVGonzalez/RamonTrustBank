using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustBank.Application.Dtos
{
    public class CloseAccountDto
    {
        [Required]
        public string AccountNumberToClose { get; set; }

        [Required]
        [EmailAddress]
        public string AdminEmail { get; set; }

        [Required]
        public string AdminPin { get; set; }//needs to be masked
    }
}
