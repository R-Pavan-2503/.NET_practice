// // // using System;

// // // class Demo
// // // {
// // //     static int count = Print("static init", 1);
// // //     int value = Print("instance init", 2);

// // //     static int Print(string msg, int x)
// // //     {
// // //         Console.WriteLine(msg);
// // //         return x;
// // //     }

// // //     public Demo() => Console.WriteLine("ctor");
// // // }

// // // class Program
// // // {
// // //     static void Main()
// // //     {
// // //         Demo d1 = new Demo();
// // //         Demo d2 = new Demo();
// // //     }
// // // }

// // namespace Restaurant
// // {
// //     class Program
// //     {
// //         static void Takeorder(object waitername)
// //         {
// //             for (int i = 0; i <= 5; i++)
// //             {
// //                 Console.WriteLine(waitername + " is taking order " + i);
// //                 Thread.Sleep(1000);
// //             }

// //         }
// //         static void Main()
// //         {
// //             Console.WriteLine("All waiters are started to taking order");
// //             // Takeorder("Waiter A");
// //             // Takeorder("Waiter B");
// //             // Takeorder("Waiter A");
// //             Thread waiter1 = new Thread(Takeorder);
// //             Thread waiter2 = new Thread(Takeorder);
// //             Thread waiter3 = new Thread(Takeorder);
// //             waiter1.Start("Waiter A");
// //             waiter2.Start("Waiter B");
// //             waiter3.Start("Waiter C");
// //             waiter1.Join();
// //             waiter2.Join();
// //             waiter3.Join();
// //             Console.WriteLine("All the orders are taken and delivered");
// //         }
// //     }
// // }

// // using System;

// // class Base
// // {
// //     protected int value = 1;

// //     public Base()
// //     {
// //         Console.WriteLine($"Base ctor → value={value}");
// //         Display(); // virtual call here!
// //         value += 2;
// //     }

// //     public virtual void Display()
// //     {
// //         Console.WriteLine($"Base.Display → value={value}");
// //     }
// // }

// // class Derived : Base
// // {
// //     public Derived()
// //     {
// //         Console.WriteLine($"Derived ctor → value={value}");
// //         value += 3;
// //     }

// //     public override void Display()
// //     {
// //         Console.WriteLine($"Derived.Display → value={value}");
// //         value++;
// //     }
// // }

// // class Program
// // {
// //     static void Main()
// //     {
// //         Derived d = new Derived();
// //         d.Display();
// //         Console.WriteLine($"Final value={d}");
// //     }
// // }


// // See https://aka.ms/new-console-template for more information
// using System.Runtime.CompilerServices;

// // Console.WriteLine("Hello, World!");
// using System;
// using System.Threading.Tasks;
// using System.Threading;

// namespace Ticketbooking
// {
//     class Program
//     {
//         static async Task Main()
//         {
//             Console.WriteLine("Welcome to ticket booking platform");
//             Console.Write("Enter yout Boarding point : ");

//             string from = Console.ReadLine();
//             Console.Write("Enter yout Destination point : ");

//             string to = Console.ReadLine();
//             Console.Write("Enter no of seats : ");

//             int seat = int.Parse(Console.ReadLine());
//             decimal ticketprice = 500.00m;
//             decimal totalprice = seat * ticketprice;
//             Console.WriteLine($"Price per seat {ticketprice}");
//             Console.WriteLine($"Total amount to be paid {totalprice}");

//             CancellationTokenSource cts = new CancellationTokenSource();
//             Console.WriteLine($"Ticket Booking from {from} to {to}");
//             Console.WriteLine($"Press C to cancel the ticket booking");

//             Task bookingtask = BookticketAsync(from, totalprice, seat, totalprice, cts.Token);

//             Task CancelTask = Task.Run(() =>
//             {
//                 char key = Console.ReadKey(true).KeyChar;
//                 if (char.ToUpper(key) == 'C')
//                 {
//                     Console.WriteLine("Ticket cancellation Requested");
//                     cts.Cancel();
//                     Console.WriteLine("Booking cancellation Requested");
//                     Console.WriteLine("Thank you for using booking platform");
//                 }
//             });

//             try
//             {
//                 await bookingtask;
//                 Console.WriteLine($"Ticket has been booked, {seat} seats from {from} to {to}");
//                 Console.WriteLine($"Total amount paid {totalprice}");
//             }
//             catch (OperationCanceledException e)
//             {
//                 Console.WriteLine("Ticket has been cancelled by the user.... ");
//                 cts.Dispose();
//             }

//             static async Task BookticketAsync(string from, decimal price, int seats, decimal total, CancellationToken token)
//             {
//                 // Console.WriteLine("Processing your booking...");
//                 // await Task.Delay(5000, token);
//                 // token.ThrowIfCancellationRequested();
//                 string[] steps =
//                 {
//                     "Connecting to server...........",
//                     "Checking the route and seat availability",
//                     "Calculating final amount",
//                     "Processing the payment",
//                     "Reloading to Confirmation page.......",
//                     "Ticket has confirmed",
//                     "Generating e-Ticket",
//                     "Sending the e-ticket to mailid.....",
//                     "Bus No and Driver Contact will be sentf to you"
//                 };
//                 foreach (var step in steps)
//                 {
//                     token.ThrowIfCancellationRequested();
//                     Console.WriteLine(step);
//                     await Task.Delay(2000);
//                 }
//             }
//         }
//     }
// }

class A
{
    public static int x = B.Get() + 1;
}

class B
{
    public static int y = A.x + 2;
    public static int Get()
    {
        return y + 3;
    }
}

class Program
{
    static void Main() => Console.WriteLine(A.x + B.y);
}