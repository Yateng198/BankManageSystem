using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManageSystem.Models
{
    internal class UserTransactionResponse
    {
        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public UserTransaction userTransaction { get; set; }

        public UserAccount account { get; set; }


        public List<UserTransaction> listTransaction { get; set; }
    }
}
