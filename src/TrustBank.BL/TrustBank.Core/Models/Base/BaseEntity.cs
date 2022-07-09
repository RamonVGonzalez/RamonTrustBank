using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustBank.Core.Models.Base
{
    public class BaseEntity
    {
        //public string Id { get; set; } = Guid.NewGuid().ToString();
        //public string DateCreated { get; set; } = DateTime.Now.ToString();
        //public string DateUpdated { get; set; } = DateTime.Now.ToString();


        [Key]
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateOfLastUpdate { get; set; }

        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateOfLastUpdate = DateTime.Now;
        }
    }
}
