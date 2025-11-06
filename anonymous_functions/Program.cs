using System;

namespace AnonymousFunctionDemo
{

    public delegate void GreetDelegate(string name);

    class Program
    {
        static void Main()
        {

            GreetDelegate greet = delegate (string name)
            {
                Console.WriteLine($"Hello, {name}! Welcome to C# Anonymous Functions.");
            };


            greet("Vinith");
        }
    }
}
