using BankManageSystem.Models;
using System.Collections.Generic;

namespace BankManageSystem.Models
{
    public class Response
    {
        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public Currency currency { get; set; }

        public List<Currency> listCurrency { get; set; }

    }
}
