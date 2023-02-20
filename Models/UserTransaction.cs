using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManageSystem.Models
{
    internal class UserTransaction
    {
        public int recordId { get; set; }
        public int userId { get; set; }
        public string cardNumber { get; set; }
        public string transactionType { get; set; }
        public DateTime transactionTime { get; set; }
    }
}
