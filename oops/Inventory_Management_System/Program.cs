using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementSystem
{

    public delegate void LowStockWarningHandler(Product product);


    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }

        public Product(int id, string name, decimal price, int quantityInStock)
        {
            Id = id;
            Name = name;
            Price = price;
            QuantityInStock = quantityInStock;
        }
    }

    class InventoryManager
    {
        private List<Product> _Products = new List<Product>();

        public event LowStockWarningHandler? OnLowStock;

        public void AddProduct(Product product)
        {
            _Products.Add(product);
        }

        public void ShowInventory()
        {
            if (_Products.Count == 0)
            {
                Console.WriteLine("No products recorded yet.");
                return;
            }

            Console.WriteLine("\n=== Current Inventory ===");
            foreach (var product in _Products)
            {
                Console.WriteLine($"Name: {product.Name}, Price: ₹{product.Price}, Quantity in Stock: {product.QuantityInStock}");
            }
            Console.WriteLine("==========================\n");
        }

        public Product? FindProductByID(int productId)
        {
            return _Products.FirstOrDefault(p => p.Id == productId);
        }

        public void SellProduct(int productId, int quantityToSell)
        {
            Product? currProduct = FindProductByID(productId);

            if (currProduct == null)
            {
                Console.WriteLine("❌ Error: Product not found");
            }
            else if (currProduct.QuantityInStock < quantityToSell)
            {
                Console.WriteLine($"❌ Error: Not enough stock for {currProduct.Name}");
            }
            else
            {
                currProduct.QuantityInStock -= quantityToSell;
                Console.WriteLine($"✅ Sold {quantityToSell} units of {currProduct.Name}. Remaining stock: {currProduct.QuantityInStock}");

                if (currProduct.QuantityInStock < 5)
                {
                    OnLowStock?.Invoke(currProduct);
                }
            }
        }

        public void RestockProduct(int productId, int quantityToAdd)
        {
            Product? currProduct = FindProductByID(productId);
            if (currProduct == null)
            {
                Console.WriteLine("❌ Error: Product not found");
            }
            else
            {
                currProduct.QuantityInStock += quantityToAdd;
                Console.WriteLine($"🔄 Restocked {quantityToAdd} units of {currProduct.Name}. New stock: {currProduct.QuantityInStock}");
            }
        }

        public void ShowInventoryValue()
        {
            if (_Products.Count == 0)
            {
                Console.WriteLine("No products available to calculate total inventory value.");
                return;
            }

            decimal totalValue = 0;
            foreach (var product in _Products)
            {
                totalValue += product.Price * product.QuantityInStock;
            }

            Console.WriteLine($"\n💰 Total Inventory Value: ₹{totalValue}\n");
        }


    }

    class Program
    {
        static void Main(string[] args)
        {
            static void HandleLowStock(Product product)
            {
                Console.WriteLine($"⚠️ WARNING: {product.Name} stock is LOW! Only {product.QuantityInStock} left!");

            }

            InventoryManager manager = new InventoryManager();

            manager.OnLowStock += HandleLowStock;

            manager.AddProduct(new Product(1, "Laptop", 60000m, 10));
            manager.AddProduct(new Product(2, "Mouse", 500m, 20));
            manager.AddProduct(new Product(3, "Keyboard", 1500m, 15));


            manager.ShowInventory();


            Console.WriteLine("Attempting to sell 5 Laptops...");
            manager.SellProduct(1, 5);
            Console.WriteLine("Attempting to sell 1 more Laptop...");
            manager.SellProduct(1, 1);

            Console.WriteLine("Attempting to sell 50 Mice...");
            manager.SellProduct(2, 50);


            Console.WriteLine("Restocking 10 Mice...");
            manager.RestockProduct(2, 10);


            manager.ShowInventory();


            manager.ShowInventoryValue();

            Console.WriteLine("Simulation complete!");
        }


    }
}
