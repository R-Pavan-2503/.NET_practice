namespace looping
{
    class Program
    {
        static void Main()
        {
            int x = 256;

            if (x < 256)
            {
                Console.WriteLine(x);
            }

            for (int i = 256; i <= 270; i++)
            {
                Console.WriteLine("Hello World");
            }
        }
    }
}