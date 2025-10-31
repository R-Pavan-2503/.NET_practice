namespace Shopping_Cart
{
    public enum OrderStatus
    {
        Pending,
        Shipped,
        Delivered
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }

    public class StoreInventory
    {
        private Dictionary<int, Product> _products = new Dictionary<int, Product>();

        public void AddProduct(Product product)
        {
            _products.Add(product.Id, product);
        }

        public Product? GetProductById(int productId)
        {
            return _products.ContainsKey(productId) ? _products[productId] : null;
        }
    }

    public class ShoppingCart
    {
        public Dictionary<Product, int> _items = new Dictionary<Product, int>();

        public void AddItem(Product product, int quantity)
        {
            if (_items.ContainsKey(product))
            {
                _items[product] += quantity;
            }
            else
            {
                _items.Add(product, quantity);
            }
        }

        public void RemoveItem(Product product)
        {
            _items.Remove(product);
        }

        public decimal CalculateTotal()
        {
            decimal totalValue = 0;
            foreach (var item in _items)
            {
                totalValue += (item.Key.Price * item.Value);
            }

            return totalValue;
        }
    }

    public class Order
    {
        public int OrderId { get; private set; }
        public Dictionary<Product, int> Items { get; private set; }
        public decimal TotalAmount { get; private set; }
        public OrderStatus Status { get; private set; }

        public Order(int orderId, Dictionary<Product, int> items, decimal totalAmount)
        {
            OrderId = orderId;
            Items = new Dictionary<Product, int>(items);
            TotalAmount = totalAmount;
            Status = OrderStatus.Pending;
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
        }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public ShoppingCart ShoppingCart { get; private set; }
        public List<Order> _Orders { get; private set; }

        public Customer(int customerId, string name)
        {
            this.CustomerId = customerId;
            this.Name = name;
            this.ShoppingCart = new ShoppingCart();
            this._Orders = new List<Order>();
        }


        public void Checkout()
        {
            decimal total = ShoppingCart.CalculateTotal();
            int newOrderId = _Orders.Count + 1;


            Order newOrder = new Order(newOrderId, ShoppingCart._items, total);


            _Orders.Add(newOrder);


            ShoppingCart = new ShoppingCart();

            Console.WriteLine($"Order #{newOrderId} placed successfully for {Name}. Total = ₹{total}");
        }

        public void ShowOrderHistory()
        {
            Console.WriteLine($"\n--- Order History for {Name} ---");
            if (_Orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
                return;
            }

            foreach (var order in _Orders)
            {
                Console.WriteLine($"Order ID: {order.OrderId} | Total: ₹{order.TotalAmount} | Status: {order.Status}");

                foreach (var item in order.Items)
                {
                    Console.WriteLine($"\t- {item.Key.Name} (x{item.Value})");
                }
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {

            StoreInventory inventory = new StoreInventory();


            Product apple = new Product(1, "Apple", 10.0m);
            Product banana = new Product(2, "Banana", 5.0m);
            Product orange = new Product(3, "Orange", 8.0m);


            inventory.AddProduct(apple);
            inventory.AddProduct(banana);
            inventory.AddProduct(orange);


            Customer alice = new Customer(1, "Alice");


            Product? appleFromInventory = inventory.GetProductById(1);
            Product? bananaFromInventory = inventory.GetProductById(2);
            Product? orangeFromInventory = inventory.GetProductById(3);


            if (appleFromInventory != null && bananaFromInventory != null)
            {
                alice.ShoppingCart.AddItem(appleFromInventory, 5);
                alice.ShoppingCart.AddItem(bananaFromInventory, 10);
            }


            alice.Checkout();


            if (orangeFromInventory != null)
            {
                alice.ShoppingCart.AddItem(orangeFromInventory, 3);
            }


            alice.Checkout();


            alice.ShowOrderHistory();
        }
    }
}
