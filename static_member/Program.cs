using System;

namespace StaticMembers
{
    static class Employee
    {
        public static string CompanyName;
        public static int EmployeeCount;

        static Employee()
        {
            CompanyName = "UPS Logistics";
            EmployeeCount = 0;
            Console.WriteLine("Welcome to UPS");
        }

        public static void DisplayEmployee()
        {
            Console.WriteLine("Employee Details");
            Console.WriteLine($"Company Name : {CompanyName}");
            Console.WriteLine($"Total Employees : {EmployeeCount}");
        }
    }

    class Staff
    {
        public string Name;
        public string Department;

        public void GetDetails()
        {
            Console.WriteLine("Enter the employee name:");
            Name = Console.ReadLine();

            Console.WriteLine("Enter the department:");
            Department = Console.ReadLine();

            Employee.EmployeeCount++;
        }

        public void DisplayDetails()
        {
            Console.WriteLine($"Name : {Name}, Department : {Department}");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter the number of employees:");
            int n = int.Parse(Console.ReadLine());

            Staff[] staffList = new Staff[n];

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"Enter details for employee {i + 1}");
                staffList[i] = new Staff();
                staffList[i].GetDetails();
            }

            Console.WriteLine("\nEmployee Details:");
            foreach (Staff s in staffList)
            {
                s.DisplayDetails();
            }

            Console.WriteLine();
            Employee.DisplayEmployee();
        }
    }
}



