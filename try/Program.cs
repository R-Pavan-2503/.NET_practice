// using System;

// namespace MyNamespace
// {
//     class Order
//     {
//         public static int A = B + 1;
//         public static int B = 5;
//     }

//     class Program
//     {
//         static void Main()
//         {
//             Console.WriteLine(Order.A);
//             Console.WriteLine(Order.B);
//         }
//     }
// }


// using System;

// namespace DelegateDemo
// {
//     public delegate int MathOperation(int a, int b);

//     class Program
//     {
//         static int Add(int a, int b) => a + b;
//         static int Multiply(int a, int b) => a * b;

//         static void Main()
//         {
//             MathOperation addOp = Add;
//             MathOperation multiplyOp = Multiply;

//             Console.WriteLine("Addition: " + addOp(5, 3));
//             Console.WriteLine("Multiplication: " + multiplyOp(5, 3));
//         }
//     }
// }

namespace Try
{
    class Demo
    {
        public static int i = 0;
        static Demo() { i += 10; }
    }
    class Program
    {
        static void Main()
        {

            Console.WriteLine(Demo.i);
        }
    }
}