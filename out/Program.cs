namespace Out
{
    class Program
    {
        public bool show(int emp, ref string name, out double basic)
        {
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

            return true;

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
            string name;
            double basic;

            emp = 101;

            Program obj = new Program();
            obj.show(emp, out name, out basic);
            Console.WriteLine("Employee Name: " + name);
            Console.WriteLine("Employee Basic Salary: " + basic);

        }


    }
}