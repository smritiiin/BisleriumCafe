using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BisleriumCafe.Data
{
    public class OrderService
    {
        public Orders orderInstance;

        public OrderService()
        {
            Orders orders = new();
            orderInstance = orders;
            orderInstance.addInsList = new List<AddIn>();
            orderInstance.coffeeList = new List<Coffees>();
            orderInstance.totalAmount = 0;
        }


        public List<Orders> GetAllOrders()
        {
            string ordersFilePath = Utils.GetAppOrdersFilePath();
            if (!File.Exists(ordersFilePath))
            {
                return new List<Orders>();
            }

            var json = File.ReadAllText(ordersFilePath);

            return JsonSerializer.Deserialize<List<Orders>>(json);
        }

        public int getOrderCount(string customerPhone)
        {
            List<Orders> orders = GetAllOrders();
            int count = 0;
            foreach (var order in orders)
            {
                if (order.customerPhone.Equals(customerPhone))
                {
                    count++;
                }
            }
            return count;
        }

        public List<Orders> GetUniqueOrdersForCustomer(string customerPhone)
        {
            List<Orders> allOrders = GetAllOrders();
            var uniqueOrders = allOrders
                .Where(o => o.customerPhone == customerPhone)
                .GroupBy(o => o.orderDate)
                .Select(group => group.First())
                .ToList();

            return uniqueOrders;
        }

        public bool checkDiscountAvailable(string customerPhone)
        {
            List<Orders> orders = GetUniqueOrdersForCustomer(customerPhone);

            DateTime currentDate = DateTime.Now;
            DateTime thresholdDate = currentDate.AddDays(-30);

            int orderCountForMonth = 0;

            foreach (var order in orders)
            {
                if (orderInstance.orderDate >= thresholdDate)
                {
                    orderCountForMonth++;
                }

            }

            if (orderCountForMonth >= 22)
                return true;
            return false;
        }
        public int GetCoffeeWithHighestPrice()
        {
            List<Coffees> coffees = orderInstance.coffeeList;
            if (coffees == null || coffees.Count == 0)
            {
                return 0;
            }

            var coffeeWithHighestPrice = coffees.OrderByDescending(c => c.CoffeePrice).FirstOrDefault();

            return coffeeWithHighestPrice.CoffeePrice;
        }


        public void SaveAllOrders()
        {
            Console.WriteLine(orderInstance);
            List<Orders> orderList = GetAllOrders();
            orderInstance.orderDate = DateTime.Now;

            orderList.Add(orderInstance);
            orderInstance = new();
            orderInstance.addInsList = new List<AddIn>();
            orderInstance.coffeeList = new List<Coffees>();

            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string appOrdersFilePath = Utils.GetAppOrdersFilePath();           
            var json = JsonSerializer.Serialize(orderList);
            Console.WriteLine(json);
            File.WriteAllText(appOrdersFilePath, json);
        }
    }
}
