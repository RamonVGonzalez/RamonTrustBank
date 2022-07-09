using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustBank.Application.Dtos
{
    public class CreateAccountDto
    {
        [Required]
        public string ProductId { get; set; }

        [Required]
        public string Pin { get; set; }

    }
}
