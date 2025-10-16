namespace BoxingUnboxing
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 12;
            double y = 12.5;
            string name = "vikas";

            //boxing
            object ob1 = x;
            object ob2 = y;
            object ob3 = name;

            //unboxing
            int x1 = (Int32)ob1;
            double y1 = (Double)ob2;
            string name1 = (string)ob3;
        }
    }
}