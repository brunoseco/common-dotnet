using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Payment.Entity.Lists
{
    internal class CustomersModel
    {
        public int totalItems { get; set; }
        public List<CustomerModel> items { get; set; }
    }
}
