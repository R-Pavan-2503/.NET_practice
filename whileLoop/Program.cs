using System;


namespace While
{
    class Program
    {
        public void show(int n)
        {
            int i = 0;
            while (i < n)
            {
                Console.WriteLine(i + "th line");
                i++;
            }
        }
        static void Main(string[] args)
        {
            int n;
            n = Convert.ToInt32(Console.ReadLine());

            Program obj = new Program();
            obj.show(n);

        }
    }
}