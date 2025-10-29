namespace taxiBooking
{
    public enum TaxiState
    {
        Available,
        OnTrip
    }

    public enum TripStatus
    {
        InProgress,
        Completed
    }


    class Taxi
    {
        public int Id { get; set; }
        public string DriverName { get; set; }
        public TaxiState CurrentState { get; set; }

        public Taxi(int id, string driverName)
        {
            this.Id = id;
            this.DriverName = driverName;
            this.CurrentState = TaxiState.Available;
        }

        public bool StartTrip()
        {
            if (CurrentState == TaxiState.Available)
            {
                CurrentState = TaxiState.OnTrip;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EndTrip()
        {
            if (CurrentState == TaxiState.OnTrip)
            {
                CurrentState = TaxiState.Available;
                return true;
            }
            return false;
        }

    }

    class TaxiFleet
    {
        private List<Taxi> _taxis = new List<Taxi>();


        public void AddTaxi(Taxi taxi)
        {
            _taxis.Add(taxi);
        }

        public Taxi? FindAvailableTaxi()
        {
            return _taxis.FirstOrDefault(t => t.CurrentState == TaxiState.Available);
        }
    }

    class BookingManager
    {
        private List<Trip> _trips = new List<Trip>();

        public Trip? BookRide(TaxiFleet taxiFleet, string customerName)
        {
            var taxi = taxiFleet.FindAvailableTaxi();

            if (taxi == null)
            {
                Console.WriteLine("Sorry, no taxis are available");
                return null;
            }
            else
            {
                taxi.StartTrip();
                Console.WriteLine($"Booked Taxi #{taxi.Id}, Driver: {taxi.DriverName}");
                Trip currentTrip = new Trip(taxi, customerName);
                _trips.Add(currentTrip);
                return currentTrip;

            }
        }

        public void EndRide(Trip trip, decimal fare)
        {
            if (trip == null)
            {
                return;
            }
            else
            {
                trip.CurrentStatus = TripStatus.Completed;
                trip.Fare = fare;
                trip.Taxi.EndTrip();
            }
        }
        public void ShowTripHistory()
        {
            Console.WriteLine("\n---- Trip History ----");

            if (_trips.Count == 0)
            {
                Console.WriteLine("No trips recorded yet.");
                return;
            }

            foreach (var trip in _trips)
            {
                Console.WriteLine($"Customer: {trip.CustomerName}, " +
                                  $"Driver: {trip.Taxi.DriverName}, " +
                                  $"Fare: ₹{trip.Fare}, " +
                                  $"Status: {trip.CurrentStatus}");
            }
        }
    }

    class Trip
    {
        public Taxi Taxi { get; set; }
        public string CustomerName { get; set; }
        public decimal Fare { get; set; }
        public TripStatus CurrentStatus { get; set; }

        public Trip(Taxi taxi, string customerName)
        {
            this.Taxi = taxi;
            this.CustomerName = customerName;
            this.Fare = 0;
            this.CurrentStatus = TripStatus.InProgress;
        }


    }
    class Program
    {
        public static void Main(string[] args)
        {
            TaxiFleet taxiFleet = new TaxiFleet();
            BookingManager bookingManager = new BookingManager();

            Taxi taxi1 = new Taxi(1, "John");
            Taxi taxi2 = new Taxi(2, "Jane");

            taxiFleet.AddTaxi(taxi1);
            taxiFleet.AddTaxi(taxi2);

            Console.WriteLine("---- Ride 1 ----");
            Trip? trip1 = bookingManager.BookRide(taxiFleet, "Alice");

            Console.WriteLine("---- Ride 2 ----");
            Trip? trip2 = bookingManager.BookRide(taxiFleet, "Bob");

            bookingManager.EndRide(trip1, 15.50m);

            Console.WriteLine("---- Ride 3 ----");
            Trip? trip3 = bookingManager.BookRide(taxiFleet, "Charlie");

            Console.WriteLine("\nSimulation complete!");

            bookingManager.ShowTripHistory();
        }
    }
}