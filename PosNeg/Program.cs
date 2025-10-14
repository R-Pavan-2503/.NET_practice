using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;


namespace PosNeg
{
    class Program
    {

        public void check(int n)
        {
            if (n > 0)
            {
                Console.WriteLine(n + " is an positive number");
            }
            else
            {
                Console.WriteLine(n + " is an negative number");
            }
        }
        static void Main(string[] args)
        {
            int n;
            Console.WriteLine("enter the n values");
            n = Convert.ToInt32(Console.ReadLine());
            Program p = new Program();
            p.check(n);

        }
    }
}