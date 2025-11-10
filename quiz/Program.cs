// using System;

// class Demo
// {
//     static int count = Print("static init", 1);
//     int value = Print("instance init", 2);

//     static int Print(string msg, int x)
//     {
//         Console.WriteLine(msg);
//         return x;
//     }

//     public Demo() => Console.WriteLine("ctor");
// }

// class Program
// {
//     static void Main()
//     {
//         Demo d1 = new Demo();
//         Demo d2 = new Demo();
//     }
// }

namespace Restaurant
{
    class Program
    {
        static void Takeorder(object waitername)
        {
            for (int i = 0; i <= 5; i++)
            {
                Console.WriteLine(waitername + " is taking order " + i);
                Thread.Sleep(1000);
            }

        }
        static void Main()
        {
            Console.WriteLine("All waiters are started to taking order");
            // Takeorder("Waiter A");
            // Takeorder("Waiter B");
            // Takeorder("Waiter A");
            Thread waiter1 = new Thread(Takeorder);
            Thread waiter2 = new Thread(Takeorder);
            Thread waiter3 = new Thread(Takeorder);
            waiter1.Start("Waiter A");
            waiter2.Start("Waiter B");
            waiter3.Start("Waiter C");
            waiter1.Join();
            waiter2.Join();
            waiter3.Join();
            Console.WriteLine("All the orders are taken and delivered");
        }
    }
}