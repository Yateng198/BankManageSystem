using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManageSystem.Models
{
    internal class UserInfoResponse
    {
        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public UserInfo user { get; set; }
        public UserAccount accout { get; set; }

        public List<UserInfo> users { get; set; }
    }
}
