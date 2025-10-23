namespace ConstructorDemo
{
    class Program
    {
        // class Constructor
        static Program()
        {
            Console.WriteLine("Static Constructor called");
        }

        // instance Constructor
        Program()
        {
            Console.WriteLine("Instance Constructor called");
        }
        static void Main(string[] args)
        {
            Program obj = new Program();
            

        }
    }
}