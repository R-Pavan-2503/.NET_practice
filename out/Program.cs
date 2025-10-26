namespace Out
{
    class Program
    {
        public void show(int emp, ref string name, ref double basic)
        {


        }

        (string name, double basic) show1(int emp)
        {
            string name;
            double basic;
            if (emp == 101)
            {
                name = "Smith";
                basic = 35000;

            }
            else if (emp == 102)
            {
                name = "Scott";
                basic = 55000;
            }
            else
            {
                name = "Kathy";
                basic = 25000;
            }

            return (name, basic);
        }
        static void Main(string[] args)
        {
            int emp;
            string name = " ";
            double basic = 0.0;

            emp = 101;

            Program obj = new Program();
            obj.show(emp, ref name, ref basic);
            Console.WriteLine("Employee Name: " + name);
            Console.WriteLine("Employee Basic Salary: " + basic);

            // obj.show1(emp);
            // var res = obj.show1(emp);
            // Type type = res.GetType();
            // Console.WriteLine("Type Name: " + type.Name);
            // Console.WriteLine("Employee Name: " + res.name);
            // Console.WriteLine("Employee Basic Salary: " + res.basic);

        }


    }
}