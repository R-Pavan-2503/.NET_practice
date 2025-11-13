namespace q1
{
    class A
    {
        public static int val = Init("A.val");


        static int Init(string s)
        {
            Console.WriteLine(s);
            return 10;
        }
    }

    class B : A
    {
        public new static int val = Init("B.val");
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine(A.val);
            Console.WriteLine(B.val);
        }
    }
}
