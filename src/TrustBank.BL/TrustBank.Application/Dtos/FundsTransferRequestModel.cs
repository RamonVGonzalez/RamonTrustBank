using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustBank.Application.Dtos
{
    public class FundsTransferRequestModel
    {
        [Required]
        public string FromAccount { get; set; }

        [Required]
        public string ToAccount { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Narration { get; set; }

        [Required]
        public string Pin { get; set; }
        
    }
}
