using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManageSystem.Models
{
    public class User
    {
        public int userId { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime DOB { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string occupation { get; set; }
        public string address_Street { get; set; }
        public string address_City { get; set; }
        private string address_Province { get; set; }
        private string address_County { get; set; }
        public string postalCode { get; set; }

    }
}
