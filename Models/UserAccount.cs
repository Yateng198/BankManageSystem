using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManageSystem.Models
{
    internal class UserAccount
    {
        public int userId { get; set; }
        public long cardNumber { get; set; }
        public double balance { get; set; }
    }
}
