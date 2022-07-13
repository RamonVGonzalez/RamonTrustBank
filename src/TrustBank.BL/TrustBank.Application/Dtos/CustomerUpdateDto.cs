using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustBank.Core.Models;

namespace TrustBank.Application.Dtos
{
    public class CustomerUpdateDto
    {
        [EmailAddress]
        public string NewEmail { get; set; }

        [RegularExpression(@"^[0][7-9][0-9]{9}")]
        public string NewPhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

        [RegularExpression(@"^[0-9]{4}")]
        public string NewPin { get; set; }

        public string ConfirmPin { get; set; }

        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
    }
}
