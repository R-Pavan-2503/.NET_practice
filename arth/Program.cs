namespace arth
{
    class Program
    {
        static void Main()
        {
            int num1, num2;

            Console.Write("ENTER FIRST NUMBER: ");
            num1 = Convert.ToInt32(Console.ReadLine());
            Console.Write("ENTER SECOND NUMBER: ");
            num2 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\n-- Arthmetic Operations --");
            Console.WriteLine("ADDITION: " + (num1 + num2));
            Console.WriteLine("SUBTRACTION: " + (num1 - num2));
            Console.WriteLine("MULTIPLICATION: " + (num1 * num2));
            Console.WriteLine("DIVISION: " + (num1 / num2));
            Console.WriteLine("MODULUS: " + (num1 % num2));
        }
    }

}