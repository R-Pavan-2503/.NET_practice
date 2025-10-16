namespace MethodOverloading
{
    class Program
    {
        public void show(int x)
        {
            Console.WriteLine("Integer value: " + x);
        }

        public void show(double y)
        {
            Console.WriteLine("Double value: " + y);
        }

        public void show(string name)
        {
            Console.WriteLine("String value: " + name);
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.show(12);
            p.show(12.5);
            p.show("vikas");
        }
    }
}