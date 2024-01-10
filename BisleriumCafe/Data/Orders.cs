using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data
{
    public  class Orders
    {
        public List<Coffees> coffeeList;
        public List<AddIn> addInsList;
        public int totalAmount;
        public int discount;
        public string customerPhone;
        public DateTime orderDate;
        public int grandTotal;
    }
}
