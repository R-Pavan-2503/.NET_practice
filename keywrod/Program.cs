using System;

namespace CompanyDemo
{
    
    abstract class Employee
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public Employee(string name, int id)
        {
            Name = name;
            Id = id;
        }

        
        public abstract double CalculateBonus(double salary);

        
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Employee ID: {Id}, Name: {Name}");
        }

        public virtual void ShowRole()
        {
            Console.WriteLine("Role: Generic Employee");
        }
    }

    
    class Manager : Employee
    {
        public string Department { get; set; }

        public Manager(string name, int id, string dept) : base(name, id)
        {
            Department = dept;
        }

        
        public override double CalculateBonus(double salary)
        {
            return salary * 0.2; 
        }

        
        public sealed override void DisplayInfo()
        {
            Console.WriteLine($"[Manager] ID: {Id}, Name: {Name}, Department: {Department}");
        }

        
        public new void ShowRole()
        {
            Console.WriteLine("Role: Manager (method hiding using 'new')");
        }
    }

    
    class SeniorManager : Manager
    {
        public int TeamSize { get; set; }

        public SeniorManager(string name, int id, string dept, int teamSize)
            : base(name, id, dept)
        {
            TeamSize = teamSize;
        }

        

        
        public new void ShowRole()
        {
            Console.WriteLine($"Role: Senior Manager (Team Size: {TeamSize})");
        }
    }

    
    sealed class Executive : Employee
    {
        public Executive(string name, int id) : base(name, id) { }

        
        public override double CalculateBonus(double salary)
        {
            return salary * 0.3; 
        }

        
        public override void DisplayInfo()
        {
            Console.WriteLine($"[Executive] ID: {Id}, Name: {Name}");
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            Employee emp = new Executive("Arjun", 101);
            emp.DisplayInfo();
            Console.WriteLine($"Bonus: {emp.CalculateBonus(50000)}\n");

            Manager mgr = new Manager("Ravi", 102, "Finance");
            mgr.DisplayInfo();
            mgr.ShowRole();
            Console.WriteLine($"Bonus: {mgr.CalculateBonus(60000)}\n");

            SeniorManager sm = new SeniorManager("Kiran", 103, "IT", 8);
            sm.DisplayInfo(); 
            sm.ShowRole();
            Console.WriteLine($"Bonus: {sm.CalculateBonus(70000)}");
        }
    }
}
