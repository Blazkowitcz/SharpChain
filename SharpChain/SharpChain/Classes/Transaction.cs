using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpChain.Classes
{
    public class Transaction
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Amount { get; set; }

        public Transaction(string from, string to, int amount)
        {
            From = from;
            To = to;
            Amount = amount;
        }
    }
}
