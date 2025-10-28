using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TaxiBookingSystem
{
    // --- 1. ENTITY & HELPER CLASSES ---

    #region Entities

    /// <summary>
    /// ABSTRACTION: Base abstract class for all system participants.
    /// INHERITANCE: User, Driver, and Owner will inherit from this.
    /// </summary>
    public abstract class Person
    {
        // ENCAPSULATION: Private fields with public properties
        private static int _personIdCounter = 1;
        public int Id { get; private set; }
        public string Username { get; private set; }
        private string Password { get; set; } // Private: Only accessible within this class

        public Person(string username, string password)
        {
            Id = _personIdCounter++;
            Username = username;
            Password = password; // Hashing should be used in a real app
        }

        /// <summary>
        /// Validates the provided password.
        /// </summary>
        public bool CheckPassword(string password)
        {
            return this.Password == password;
        }

        /// <summary>
        /// ABSTRACTION / POLYMORPHISM: 
        /// Abstract method forces all derived classes to implement their own menu.
        /// </summary>
        public abstract void ShowMenu();
    }

    /// <summary>
    /// INHERITANCE: User inherits from Person.
    /// Represents a customer who can book rides.
    /// </summary>
    public class User : Person
    {
        public List<Trip> MyTrips { get; private set; }

        public User(string username, string password) : base(username, password)
        {
            MyTrips = new List<Trip>();
        }

        /// <summary>
        /// POLYMORPHISM: Overriding the abstract ShowMenu method.
        /// </summary>
        public override void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine($"--- Welcome User: {Username} ---");
            bool stayLoggedIn = true;
            while (stayLoggedIn)
            {
                Console.WriteLine("\n1. Book a Cab");
                Console.WriteLine("2. View My Trip History");
                Console.WriteLine("3. Logout");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        BookCab();
                        break;
                    case "2":
                        ViewMyTripHistory();
                        break;
                    case "3":
                        stayLoggedIn = false;
                        Console.WriteLine("Logging out...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the entire cab booking flow for the user.
        /// </summary>
        private void BookCab()
        {
            Console.Clear();
            Console.WriteLine("--- Book a Cab ---");

            // 1. Get Pickup Location
            int pickupPoint = Helper.GetValidPoint("Enter Pickup Point (1-10): ");

            // 2. Get Drop Location
            int dropPoint = Helper.GetValidPoint("Enter Drop Point (1-10): ");

            if (pickupPoint == dropPoint)
            {
                Console.WriteLine("Pickup and Drop points cannot be the same.");
                return;
            }

            // 3. Find available cabs
            // This is a list of tuples: (Car, DistanceToPickup)
            var availableCabs = BookingService.FindNearbyCabs(pickupPoint, Database.AllCars);

            if (!availableCabs.Any())
            {
                Console.WriteLine("Sorry, no cabs are available at the moment.");
                return;
            }

            // 4. Display available cabs
            Console.WriteLine("\nAvailable Cabs (Nearest First):");
            int cost = BookingService.CalculateCost(pickupPoint, dropPoint);
            for (int i = 0; i < availableCabs.Count; i++)
            {
                var (car, distance) = availableCabs[i];
                Console.WriteLine($"  {i + 1}. Car: {car.CarName} (Driver: {car.AssignedDriver.Username})");
                Console.WriteLine($"     Current Location: Point {car.CurrentPosition}");
                Console.WriteLine($"     Distance to you: {distance} unit(s)");
                Console.WriteLine($"     Estimated Trip Cost: ${cost}");
                Console.WriteLine("     --------------------");
            }

            // 5. User selects a cab
            Console.Write($"\nEnter choice (1-{availableCabs.Count}) or 0 to cancel: ");
            if (!int.TryParse(Console.ReadLine(), out int cabChoice) || cabChoice <= 0 || cabChoice > availableCabs.Count)
            {
                Console.WriteLine("Booking cancelled.");
                return;
            }

            // 6. Book the trip
            Car selectedCar = availableCabs[cabChoice - 1].Car;
            Trip newTrip = new Trip(this, selectedCar, pickupPoint, dropPoint, cost);

            // Add to all trip lists
            Database.AllTrips.Add(newTrip);
            this.MyTrips.Add(newTrip);
            selectedCar.AssignedDriver.MyTrips.Add(newTrip);

            // Update car state
            selectedCar.StartTrip();

            // Simulate trip
            Console.WriteLine($"\nBooking confirmed! {selectedCar.CarName} is on the way.");
            Console.WriteLine("Simulating trip...");
            Thread.Sleep(2000); // Wait 2 seconds
            Console.WriteLine($"Trip from {pickupPoint} to {dropPoint} completed!");

            // Complete trip
            selectedCar.EndTrip(dropPoint);
            Console.WriteLine($"Total cost: ${cost}. Your driver was {selectedCar.AssignedDriver.Username}.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        /// <summary>
        /// Displays the trip history for this user only.
        /// </summary>
        private void ViewMyTripHistory()
        {
            Console.Clear();
            Console.WriteLine("--- My Trip History ---");
            if (!MyTrips.Any())
            {
                Console.WriteLine("You have no trip history.");
                return;
            }

            foreach (var trip in MyTrips)
            {
                trip.DisplayDetails();
            }
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }

    /// <summary>
    /// INHERITANCE: Driver inherits from Person.
    /// Represents a driver assigned to a single car.
    /// </summary>
    public class Driver : Person
    {
        // ENCAPSULATION: Public property, but setting is controlled
        public Car AssignedCar { get; private set; }
        public List<Trip> MyTrips { get; private set; }

        public Driver(string username, string password) : base(username, password)
        {
            MyTrips = new List<Trip>();
        }

        /// <summary>
        /// Links a car to this driver.
        /// </summary>
        public void AssignCar(Car car)
        {
            AssignedCar = car;
        }

        /// <summary>
        /// POLYMORPHISM: Overriding the abstract ShowMenu method.
        /// </summary>
        public override void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine($"--- Welcome Driver: {Username} ---");
            bool stayLoggedIn = true;
            while (stayLoggedIn)
            {
                Console.WriteLine($"\nMy Car: {AssignedCar.CarName} (ID: {AssignedCar.Id})");
                Console.WriteLine($"Current Status: {(AssignedCar.IsAvailable ? "Available" : "On Trip")}");
                Console.WriteLine($"Current Location: Point {AssignedCar.CurrentPosition}");

                Console.WriteLine("\n1. View My Trip History");
                Console.WriteLine("2. Logout");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewMyTripHistory();
                        break;
                    case "2":
                        stayLoggedIn = false;
                        Console.WriteLine("Logging out...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        /// <summary>
        /// Displays trip history for this driver only.
        /// </summary>
        private void ViewMyTripHistory()
        {
            Console.Clear();
            Console.WriteLine("--- My Trip History ---");
            if (!MyTrips.Any())
            {
                Console.WriteLine("You have no trip history.");
                return;
            }

            foreach (var trip in MyTrips)
            {
                trip.DisplayDetails();
            }
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }

    /// <summary>
    /// INHERITANCE: Owner inherits from Person.
    /// Represents an admin who can manage cars and view all data.
    /// </summary>
    public class Owner : Person
    {
        public Owner(string username, string password) : base(username, password) { }

        /// <summary>
        /// POLYMORPHISM: Overriding the abstract ShowMenu method.
        /// </summary>
        public override void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine($"--- Welcome Owner: {Username} ---");
            bool stayLoggedIn = true;
            while (stayLoggedIn)
            {
                Console.WriteLine("\n1. Add New Car & Driver");
                Console.WriteLine("2. View All Car Status");
                Console.WriteLine("3. View All Trip History");
                Console.WriteLine("4. Logout");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewCarAndDriver();
                        break;
                    case "2":
                        ViewAllCarStatus();
                        break;
                    case "3":
                        ViewAllTripHistory();
                        break;
                    case "4":
                        stayLoggedIn = false;
                        Console.WriteLine("Logging out...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        /// <summary>
        /// Adds a new car and a new driver, linking them together.
        /// </summary>
        private void AddNewCarAndDriver()
        {
            Console.Clear();
            Console.WriteLine("--- Add New Car & Driver ---");

            // 1. Get Driver Details
            Console.Write("Enter new Driver's username: ");
            string driverUser = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(driverUser) || Database.FindPerson(driverUser) != null)
            {
                Console.WriteLine("Username is empty or already taken.");
                return;
            }
            Console.Write("Enter new Driver's password: ");
            string driverPass = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(driverPass))
            {
                Console.WriteLine("Password cannot be empty.");
                return;
            }

            // 2. Get Car Details
            Console.Write("Enter new Car's name (e.g., 'Toyota Camry'): ");
            string carName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(carName))
            {
                Console.WriteLine("Car name cannot be empty.");
                return;
            }
            int startPoint = Helper.GetValidPoint("Enter Car's starting point (1-10): ");

            // 3. Create and link objects
            Driver newDriver = new Driver(driverUser, driverPass);
            Car newCar = new Car(carName, startPoint, newDriver);
            newDriver.AssignCar(newCar);

            // 4. Add to database
            Database.AllPeople.Add(newDriver);
            Database.AllCars.Add(newCar);

            Console.WriteLine($"\nSuccess! Driver '{driverUser}' and Car '{carName}' (ID: {newCar.Id}) have been added.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        /// <summary>
        /// Displays status of all cars in the system.
        /// </summary>
        private void ViewAllCarStatus()
        {
            Console.Clear();
            Console.WriteLine("--- All Car Status ---");
            if (!Database.AllCars.Any())
            {
                Console.WriteLine("There are no cars in the system.");
                return;
            }

            foreach (var car in Database.AllCars.OrderBy(c => c.Id))
            {
                Console.WriteLine($"  Car ID: {car.Id}");
                Console.WriteLine($"  Name: {car.CarName}");
                Console.WriteLine($"  Driver: {car.AssignedDriver.Username}");
                Console.WriteLine($"  Status: {(car.IsAvailable ? "Available" : "On Trip")}");
                Console.WriteLine($"  Location: Point {car.CurrentPosition}");
                Console.WriteLine("  --------------------");
            }
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        /// <summary>
        /// Displays all trips from all users.
        /// </summary>
        private void ViewAllTripHistory()
        {
            Console.Clear();
            Console.WriteLine("--- All Trip History ---");
            if (!Database.AllTrips.Any())
            {
                Console.WriteLine("There is no trip history in the system.");
                return;
            }

            foreach (var trip in Database.AllTrips.OrderBy(t => t.Id))
            {
                trip.DisplayDetails();
            }
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }

    /// <summary>
    /// ENCAPSULATION: Represents a single car, managing its own state.
    /// </summary>
    public class Car
    {
        private static int _carIdCounter = 1;
        public int Id { get; private set; }
        public string CarName { get; private set; }
        public int CurrentPosition { get; private set; }
        public bool IsAvailable { get; private set; }
        public Driver AssignedDriver { get; private set; }

        public Car(string carName, int startPosition, Driver driver)
        {
            Id = _carIdCounter++;
            CarName = carName;
            CurrentPosition = startPosition;
            AssignedDriver = driver;
            IsAvailable = true; // Available by default
        }

        // ENCAPSULATION: Methods modify the object's private state.
        public void StartTrip()
        {
            IsAvailable = false;
        }

        public void EndTrip(int newPosition)
        {
            IsAvailable = true;
            CurrentPosition = newPosition;
        }
    }

    /// <summary>
    /// ENCAPSULATION: Represents a single completed trip, holding all data.
    /// </summary>
    public class Trip
    {
        private static int _tripIdCounter = 1;
        public int Id { get; private set; }
        public User BookedUser { get; private set; }
        public Car BookedCar { get; private set; }
        public Driver BookedDriver { get; private set; }
        public int PickupPoint { get; private set; }
        public int DropPoint { get; private set; }
        public int Cost { get; private set; }

        public Trip(User user, Car car, int pickup, int drop, int cost)
        {
            Id = _tripIdCounter++;
            BookedUser = user;
            BookedCar = car;
            BookedDriver = car.AssignedDriver;
            PickupPoint = pickup;
            DropPoint = drop;
            Cost = cost;
        }

        /// <summary>
        /// Helper method to print trip details.
        /// </summary>
        public void DisplayDetails()
        {
            Console.WriteLine($"  Trip ID: {Id}");
            Console.WriteLine($"  User: {BookedUser.Username}");
            Console.WriteLine($"  Driver: {BookedDriver.Username} (Car: {BookedCar.CarName})");
            Console.WriteLine($"  From: Point {PickupPoint} To: Point {DropPoint}");
            Console.WriteLine($"  Cost: ${Cost}");
            Console.WriteLine("  --------------------");
        }
    }

    #endregion

    // --- 2. SERVICES & DATABASE ---

    #region Services

    /// <summary>
    /// Static service class to handle booking logic.
    /// </summary>
    public static class BookingService
    {
        /// <summary>
        /// Calculates the cost of a trip.
        /// </summary>
        public static int CalculateCost(int pickup, int drop)
        {
            return Math.Abs(pickup - drop);
        }

        /// <summary>
        /// Finds all available cabs and returns them sorted by distance to the pickup point.
        /// </summary>
        /// <returns>A list of tuples, each containing the Car and its distance to the pickup point.</returns>
        public static List<(Car Car, int Distance)> FindNearbyCabs(int pickupLocation, List<Car> allCars)
        {
            return allCars
                .Where(car => car.IsAvailable)
                .Select(car => new
                {
                    Car = car,
                    Distance = Math.Abs(car.CurrentPosition - pickupLocation)
                })
                .OrderBy(c => c.Distance)
                .Select(c => (c.Car, c.Distance)) // Convert anonymous type to tuple
                .ToList();
        }
    }

    /// <summary>
    /// Static class acting as an in-memory database.
    /// </summary>
    public static class Database
    {
        // POLYMORPHISM: Storing User, Driver, and Owner in the same list.
        public static List<Person> AllPeople { get; private set; }
        public static List<Car> AllCars { get; private set; }
        public static List<Trip> AllTrips { get; private set; }

        static Database()
        {
            AllPeople = new List<Person>();
            AllCars = new List<Car>();
            AllTrips = new List<Trip>();
        }

        /// <summary>
        /// Sets up the initial data (1 owner, 5 cars/drivers).
        /// </summary>
        public static void Initialize()
        {
            // 1. Add Owner
            Owner owner = new Owner("owner", "pass");
            AllPeople.Add(owner);

            // 2. Add 5 initial Cars and Drivers
            AddInitialCarAndDriver("driver1", "pass", "Honda Civic", 1);
            AddInitialCarAndDriver("driver2", "pass", "Toyota Prius", 3);
            AddInitialCarAndDriver("driver3", "pass", "Ford Focus", 5);
            AddInitialCarAndDriver("driver4", "pass", "Tesla Model 3", 8);
            AddInitialCarAndDriver("driver5", "pass", "Subaru Outback", 10);

            // 3. Add a test user
            User testUser = new User("user", "pass");
            AllPeople.Add(testUser);
        }

        /// <summary>
        /// Helper to create and link a new car and driver during initialization.
        /// </summary>
        private static void AddInitialCarAndDriver(string driverUser, string driverPass, string carName, int startPoint)
        {
            Driver driver = new Driver(driverUser, driverPass);
            Car car = new Car(carName, startPoint, driver);
            driver.AssignCar(car); // Link them

            AllPeople.Add(driver);
            AllCars.Add(car);
        }

        /// <summary>
        /// Finds any person by username (case-insensitive).
        /// </summary>
        public static Person FindPerson(string username)
        {
            return AllPeople.FirstOrDefault(p => p.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <summary>
    /// Static helper class for common tasks like input validation.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Prompts the user until they enter a valid point (1-10).
        /// </summary>
        public static int GetValidPoint(string message)
        {
            int point;
            while (true)
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine(), out point) && point >= 1 && point <= 10)
                {
                    return point;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 10.");
                }
            }
        }
    }

    #endregion

    // --- 3. MAIN APPLICATION ---

    #region Main Program

    /// <summary>
    /// Main entry point for the console application.
    /// </summary>
    class Program
    {
        private static Person _loggedInPerson = null;

        static void Main(string[] args)
        {
            // Set up initial data
            Database.Initialize();
            Console.WriteLine("Welcome to the Console Taxi Booking System!");

            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("\n--- Main Menu ---");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register New User");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        HandleLogin();
                        break;
                    case "2":
                        HandleRegistration();
                        break;
                    case "3":
                        isRunning = false;
                        Console.WriteLine("Thank you for using the system. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                // If login was successful, show the person's specific menu
                if (_loggedInPerson != null)
                {
                    // !-- POLYMORPHISM IN ACTION --!
                    // We call ShowMenu() on a 'Person' object.
                    // The runtime determines which *specific* menu to show
                    // (User.ShowMenu, Driver.ShowMenu, or Owner.ShowMenu).
                    _loggedInPerson.ShowMenu();

                    // After ShowMenu() returns (i.e., user logs out),
                    // we set them back to null.
                    _loggedInPerson = null;
                    Console.Clear();
                    Console.WriteLine("You have been logged out.");
                }
            }
        }

        /// <summary>
        /// Handles the login process.
        /// </summary>
        private static void HandleLogin()
        {
            Console.Clear();
            Console.WriteLine("--- Login ---");
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            Person person = Database.FindPerson(username);

            if (person != null && person.CheckPassword(password))
            {
                Console.WriteLine($"Login successful! Welcome, {person.Username}.");
                _loggedInPerson = person; // Set the globally tracked logged-in person
            }
            else
            {
                Console.WriteLine("Invalid username or password.");
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Handles new user registration. (Owners/Drivers are added by Owner)
        /// </summary>
        private static void HandleRegistration()
        {
            Console.Clear();
            Console.WriteLine("--- Register New User ---");
            Console.Write("Enter new Username: ");
            string username = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Username cannot be empty.");
                return;
            }

            if (Database.FindPerson(username) != null)
            {
                Console.WriteLine("This username is already taken. Please try another.");
                return;
            }

            Console.Write("Enter new Password: ");
            string password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Password cannot be empty.");
                return;
            }

            // Create and add the new user
            User newUser = new User(username, password);
            Database.AllPeople.Add(newUser);

            Console.WriteLine($"Registration successful for user '{username}'. You can now log in.");
            Thread.Sleep(1500);
        }
    }

    #endregion
}

