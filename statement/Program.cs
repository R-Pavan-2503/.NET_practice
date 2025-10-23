namespace statement
{
    class Program
    {
        static void Main()
        {
            int Temperature = 28;
            if (Temperature > 25)
            {
                if (Temperature < 15)
                {
                    Console.WriteLine("It's a cold day");
                }
                else if (Temperature >= 15 && Temperature >= 25)
                {
                    Console.WriteLine("It's a normal day");
                }
            }
            else
            {
                Console.WriteLine("It's a hot day");
            }

        }
    }
}