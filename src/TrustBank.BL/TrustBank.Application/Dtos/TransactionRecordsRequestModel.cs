using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustBank.Core.Models.Enums;

namespace TrustBank.Application.Dtos
{
    public class TransactionRecordsRequestModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public int pageNumber { get; set; }
        public int RecordsPerPage { get; set; }
        
    }
}
