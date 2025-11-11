using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
namespace StudentDetails
{
    class Students
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public int Marks { get; set; }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter total Count of Students");
            int count = int.Parse(Console.ReadLine());

            List<Students> students = new List<Students>();

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"Enter students details {i + 1}");
                Console.WriteLine("Name ; ");
                string Name = Console.ReadLine();

                Console.WriteLine("Deaprtment : ");
                string Dept = Console.ReadLine();
                Console.WriteLine("Marks : ");
                int Marks = int.Parse(Console.ReadLine());

                students.Add(new Students { Name = name, Department = Dept, Marks = Marks });

            }

            Console.Write("Enter Students Cutoff Marks : ");
            int Cutoff = int.Parse(Console.ReadLine());

            var result = from s in students where s.Marks >= Cutoff select new { s.Name, s.Department, s.Marks };

            Console.WriteLine($"students Details with marks >= {cutoff} ");

            if (result.Any())
            {
                foreach (var s in students)
                {
                    Console.WriteLine($"Name : {s.Name} , Department : {s.Department} , Marks ; {s.Marks}");
                }
            }
            else
            {
                Console.WriteLine("No Students found of given cutoff");
            }

        }
    }
}