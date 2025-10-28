using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCabApp
{
    // ------------------------------
    // Abstract Account base
    // ------------------------------
    abstract class Account
    {
        public string Username { get; protected set; }
        private string _password;

        protected Account(string username, string password)
        {
            Username = username;
            _password = password;
        }

        public bool CheckPassword(string password) => _password == password;

        public virtual void DisplayMenu()
        {
            Console.WriteLine("Base account - no menu.");
        }
    }

    // ------------------------------
    // Person
    // ------------------------------
    class Person
    {
        public string Name { get; set; }
        public string Contact { get; set; }

        public Person(string name, string contact)
        {
            Name = name;
            Contact = contact;
        }
    }

    // ------------------------------
    // Customer
    // ------------------------------
    class Customer : Account
    {
        public Person Profile { get; private set; }
        public List<Trip> TripHistory { get; } = new List<Trip>();

        public Customer(string username, string password, string name, string contact)
            : base(username, password)
        {
            Profile = new Person(name, contact);
        }

        public override void DisplayMenu()
        {
            Console.WriteLine($"Customer menu for {Profile.Name} ({Username})");
        }
    }

    // ------------------------------
    // Driver
    // ------------------------------
    class Driver : Account
    {
        public Person Profile { get; private set; }
        public List<Trip> Trips { get; } = new List<Trip>();
        public Car AssignedCar { get; set; }

        public Driver(string username, string password, string name, string contact)
            : base(username, password)
        {
            Profile = new Person(name, contact);
        }

        public new void DisplayMenu()
        {
            Console.WriteLine($"Driver menu for {Profile.Name} ({Username}) - Car: {(AssignedCar != null ? AssignedCar.Name : "None")}");
        }
    }

    // ------------------------------
    // Owner
    // ------------------------------
    sealed class Owner : Account
    {
        public Person Profile { get; private set; }

        public Owner(string username, string password, string name, string contact)
            : base(username, password)
        {
            Profile = new Person(name, contact);
        }

        public override void DisplayMenu()
        {
            Console.WriteLine($"Owner menu for {Profile.Name} ({Username})");
        }
    }

    // ------------------------------
    // Car
    // ------------------------------
    class Car
    {
        public string Id { get; private set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public bool IsAvailable { get; private set; } = true;
        public Driver Driver { get; set; }

        public Car(string id, string name, int position)
        {
            Id = id;
            Name = name;
            Position = position;
            IsAvailable = true;
        }

        public void StartTrip() => IsAvailable = false;

        public void EndTrip(int newPosition)
        {
            IsAvailable = true;
            Position = newPosition;
        }

        public override string ToString()
        {
            return $"{Name} (Id:{Id}) Pos:{Position} Available:{IsAvailable} Driver:{(Driver != null ? Driver.Profile.Name : "Unassigned")}";
        }
    }

    // ------------------------------
    // Trip
    // ------------------------------
    class Trip
    {
        public string TripId { get; private set; }
        public Customer Customer { get; private set; }
        public Driver Driver { get; private set; }
        public Car Car { get; private set; }
        public int Pickup { get; private set; }
        public int Drop { get; private set; }
        public int Price { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? EndedAt { get; private set; }

        public Trip(string tripId, Customer customer, Driver driver, Car car, int pickup, int drop, int price)
        {
            TripId = tripId;
            Customer = customer;
            Driver = driver;
            Car = car;
            Pickup = pickup;
            Drop = drop;
            Price = price;
            StartedAt = DateTime.Now;
        }

        public void CompleteTrip() => EndedAt = DateTime.Now;

        public override string ToString()
        {
            string ended = EndedAt.HasValue ? EndedAt.Value.ToString("g") : "In-Progress";
            return $"Trip:{TripId} Cust:{Customer.Profile.Name} Driver:{Driver.Profile.Name} Car:{Car.Name} P:{Pickup} D:{Drop} Price:{Price} Start:{StartedAt:g} End:{ended}";
        }
    }

    // ------------------------------
    // Data Store
    // ------------------------------
    static class DataStore
    {
        public static List<Customer> Customers { get; } = new();
        public static List<Driver> Drivers { get; } = new();
        public static List<Car> Cars { get; } = new();
        public static List<Owner> Owners { get; } = new();
        public static List<Trip> Trips { get; } = new();

        public static Account FindAccount(string username)
        {
            return Customers.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                ?? Drivers.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                ?? Owners.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }

    // ------------------------------
    // Program
    // ------------------------------
    class Program
    {
        static void Main()
        {
            SeedInitialData();
            Console.WriteLine("=== Welcome to ConsoleCab (Demo) ===");
            MainLoop();
        }

        static void MainLoop()
        {
            while (true)
            {
                Console.WriteLine("\nSelect:");
                Console.WriteLine("1) Register (Customer)");
                Console.WriteLine("2) Register (Owner)");
                Console.WriteLine("3) Register (Driver + Assign Car)");
                Console.WriteLine("4) Login");
                Console.WriteLine("5) Exit");
                Console.Write("Choice: ");
                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": RegisterCustomer(); break;
                    case "2": RegisterOwner(); break;
                    case "3": AddDriverAndAssignFlow(); break;
                    case "4": LoginFlow(); break;
                    case "5": Console.WriteLine("Goodbye!"); return;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        // ----------------------------
        // Registration
        // ----------------------------
        static void RegisterCustomer()
        {
            Console.WriteLine("\n--- Customer Registration ---");
            string username = ReadNonEmpty("Choose username: ");
            if (DataStore.FindAccount(username) != null) { Console.WriteLine("Username exists."); return; }
            string password = ReadPassword("Choose password: ");
            string name = ReadNonEmpty("Full name: ");
            string contact = ReadNonEmpty("Contact: ");
            DataStore.Customers.Add(new Customer(username, password, name, contact));
            Console.WriteLine($"Customer '{username}' created!");
        }

        static void RegisterOwner()
        {
            Console.WriteLine("\n--- Owner Registration ---");
            string username = ReadNonEmpty("Choose username: ");
            if (DataStore.FindAccount(username) != null) { Console.WriteLine("Username exists."); return; }
            string password = ReadPassword("Choose password: ");
            string name = ReadNonEmpty("Full name: ");
            string contact = ReadNonEmpty("Contact: ");
            DataStore.Owners.Add(new Owner(username, password, name, contact));
            Console.WriteLine($"Owner '{username}' registered successfully!");
        }

        // ----------------------------
        // Login Flow
        // ----------------------------
        static void LoginFlow()
        {
            Console.WriteLine("\n--- Login ---");
            string username = ReadNonEmpty("Username: ");
            var acc = DataStore.FindAccount(username);
            if (acc == null) { Console.WriteLine("Account not found."); return; }
            string password = ReadPassword("Password: ");
            if (!acc.CheckPassword(password)) { Console.WriteLine("Invalid password."); return; }

            Console.WriteLine($"Welcome, {username}!");
            if (acc is Customer c) CustomerMenu(c);
            else if (acc is Driver d) DriverMenu(d);
            else if (acc is Owner o) OwnerMenu(o);
        }

        // ----------------------------
        // Customer Menu
        // ----------------------------
        static void CustomerMenu(Customer customer)
        {
            while (true)
            {
                Console.WriteLine($"\n--- Customer: {customer.Profile.Name} ---");
                Console.WriteLine("1) Book a Cab");
                Console.WriteLine("2) View My Trips");
                Console.WriteLine("3) Logout");
                Console.Write("Choice: ");
                var ch = Console.ReadLine()?.Trim();

                switch (ch)
                {
                    case "1": BookCabFlow(customer); break;
                    case "2": ViewCustomerTrips(customer); break;
                    case "3": Console.WriteLine("Logging out..."); return;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        // ----------------------------
        // Driver Menu
        // ----------------------------
        static void DriverMenu(Driver driver)
        {
            while (true)
            {
                driver.DisplayMenu();
                Console.WriteLine("1) View My Trips");
                Console.WriteLine("2) Logout");
                Console.Write("Choice: ");
                var ch = Console.ReadLine()?.Trim();
                if (ch == "1") ViewDriverTrips(driver);
                else if (ch == "2") return;
                else Console.WriteLine("Invalid choice.");
            }
        }

        // ----------------------------
        // Owner Menu
        // ----------------------------
        static void OwnerMenu(Owner owner)
        {
            while (true)
            {
                owner.DisplayMenu();
                Console.WriteLine("1) View All Trips");
                Console.WriteLine("2) View Car Status");
                Console.WriteLine("3) Add Car");
                Console.WriteLine("4) Add Driver & Assign to Car");
                Console.WriteLine("5) Logout");
                Console.Write("Choice: ");
                var ch = Console.ReadLine()?.Trim();

                switch (ch)
                {
                    case "1": ViewAllTrips(); break;
                    case "2": ViewCarStatus(); break;
                    case "3": AddCarFlow(); break;
                    case "4": AddDriverAndAssignFlow(); break;
                    case "5": return;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        // ----------------------------
        // Booking Flow
        // ----------------------------
        static void BookCabFlow(Customer customer)
        {
            Console.WriteLine("\n--- Book a Cab ---");
            int pickup = ReadIntInRange("Pickup (1-10): ", 1, 10);
            int drop = ReadIntInRange("Drop (1-10): ", 1, 10);
            if (pickup == drop) { Console.WriteLine("Pickup and drop cannot be same."); return; }

            var availableCars = DataStore.Cars.Where(c => c.IsAvailable).ToList();
            if (!availableCars.Any()) { Console.WriteLine("No cars available."); return; }

            var sorted = availableCars
                .Select(car => new { car, distance = Math.Abs(car.Position - pickup), price = Math.Abs(pickup - drop) })
                .OrderBy(x => x.distance)
                .ToList();

            Console.WriteLine("Available cars:");
            for (int i = 0; i < sorted.Count; i++)
                Console.WriteLine($"{i + 1}) {sorted[i].car.Name} (Id:{sorted[i].car.Id}) Pos:{sorted[i].car.Position} Driver:{(sorted[i].car.Driver != null ? sorted[i].car.Driver.Profile.Name : "Unassigned")} Dist:{sorted[i].distance} Price:{sorted[i].price}");

            int sel = ReadIntInRange($"Select car (1-{sorted.Count}) or 0 cancel: ", 0, sorted.Count);
            if (sel == 0) return;

            var chosen = sorted[sel - 1].car;
            if (chosen.Driver == null) { Console.WriteLine("Car has no driver assigned."); return; }

            chosen.StartTrip();
            var tripId = $"T{DataStore.Trips.Count + 1:000}";
            var trip = new Trip(tripId, customer, chosen.Driver, chosen, pickup, drop, Math.Abs(pickup - drop));
            DataStore.Trips.Add(trip);
            customer.TripHistory.Add(trip);
            chosen.Driver.Trips.Add(trip);

            Console.WriteLine($"Booked {chosen.Name} with {chosen.Driver.Profile.Name}. TripId: {trip.TripId}");
            Console.WriteLine("Press Enter to complete trip...");
            Console.ReadLine();
            trip.CompleteTrip();
            chosen.EndTrip(drop);
            Console.WriteLine($"Trip {trip.TripId} completed!");
        }

        // ----------------------------
        // Viewing Functions
        // ----------------------------
        static void ViewCustomerTrips(Customer c)
        {
            Console.WriteLine($"\n--- Trips for {c.Profile.Name} ---");
            if (!c.TripHistory.Any()) Console.WriteLine("No trips yet.");
            else foreach (var t in c.TripHistory) Console.WriteLine(t);
        }

        static void ViewDriverTrips(Driver d)
        {
            Console.WriteLine($"\n--- Trips for Driver {d.Profile.Name} ---");
            if (!d.Trips.Any()) Console.WriteLine("No trips yet.");
            else foreach (var t in d.Trips) Console.WriteLine(t);
        }

        static void ViewAllTrips()
        {
            Console.WriteLine("\n--- All Trips ---");
            if (!DataStore.Trips.Any()) Console.WriteLine("No trips yet.");
            else foreach (var t in DataStore.Trips) Console.WriteLine(t);
        }

        static void ViewCarStatus()
        {
            Console.WriteLine("\n--- Cars Status ---");
            foreach (var c in DataStore.Cars) Console.WriteLine(c);
        }

        // ----------------------------
        // Owner Actions
        // ----------------------------
        static void AddCarFlow()
        {
            Console.WriteLine("\n--- Add Car ---");
            string id = ReadNonEmpty("Car Id: ");
            if (DataStore.Cars.Any(c => c.Id.Equals(id, StringComparison.OrdinalIgnoreCase))) { Console.WriteLine("Id exists."); return; }
            string name = ReadNonEmpty("Car name/model: ");
            int pos = ReadIntInRange("Initial position (1-10): ", 1, 10);
            var car = new Car(id, name, pos);
            DataStore.Cars.Add(car);
            Console.WriteLine($"Car {car.Name} added!");
        }

        static void AddDriverAndAssignFlow()
        {
            Console.WriteLine("\n--- Register Driver & Assign Car ---");
            string username = ReadNonEmpty("Driver username: ");
            if (DataStore.FindAccount(username) != null) { Console.WriteLine("Username exists."); return; }
            string password = ReadPassword("Driver password: ");
            string name = ReadNonEmpty("Driver full name: ");
            string contact = ReadNonEmpty("Driver contact: ");
            var driver = new Driver(username, password, name, contact);

            Console.WriteLine("Available cars:");
            for (int i = 0; i < DataStore.Cars.Count; i++)
                Console.WriteLine($"{i + 1}) {DataStore.Cars[i].Name} Id:{DataStore.Cars[i].Id} Driver:{(DataStore.Cars[i].Driver != null ? DataStore.Cars[i].Driver.Profile.Name : "None")}");

            int sel = ReadIntInRange($"Select car (1-{DataStore.Cars.Count}) or 0 cancel: ", 0, DataStore.Cars.Count);
            if (sel == 0) return;

            var car = DataStore.Cars[sel - 1];
            driver.AssignedCar = car;
            car.Driver = driver;
            DataStore.Drivers.Add(driver);
            Console.WriteLine($"Driver {driver.Profile.Name} assigned to {car.Name}!");
        }

        // ----------------------------
        // Seed
        // ----------------------------
        static void SeedInitialData()
        {
            var owner = new Owner("owner", "owner", "Super Owner", "owner@example.com");
            DataStore.Owners.Add(owner);

            var c1 = new Car("C1", "Swift-101", 2);
            var c2 = new Car("C2", "Dzire-202", 5);
            var c3 = new Car("C3", "Alto-303", 1);
            var c4 = new Car("C4", "Innova-404", 8);
            var c5 = new Car("C5", "Baleno-505", 10);
            DataStore.Cars.AddRange(new[] { c1, c2, c3, c4, c5 });

            var d1 = new Driver("driver1", "pass1", "Ramesh", "9990001");
            var d2 = new Driver("driver2", "pass2", "Suresh", "9990002");
            var d3 = new Driver("driver3", "pass3", "Mahesh", "9990003");
            d1.AssignedCar = c1; c1.Driver = d1;
            d2.AssignedCar = c2; c2.Driver = d2;
            d3.AssignedCar = c4; c4.Driver = d3;
            DataStore.Drivers.AddRange(new[] { d1, d2, d3 });

            DataStore.Customers.Add(new Customer("alice", "alice123", "Alice", "alice@example.com"));
            Console.WriteLine("Seeded: owner, 5 cars, 3 drivers, 1 customer.");
        }

        // ----------------------------
        // Helpers
        // ----------------------------
        static string ReadNonEmpty(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(s)) return s;
                Console.WriteLine("Cannot be empty.");
            }
        }

        static string ReadPassword(string prompt) => ReadNonEmpty(prompt);

        static int ReadIntInRange(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int v) && v >= min && v <= max) return v;
                Console.WriteLine($"Enter number {min}-{max}");
            }
        }
    }

    static class Extensions
    {
        public static void AddRange<T>(this List<T> list, IEnumerable<T> items)
        {
            foreach (var i in items) list.Add(i);
        }
    }
}
