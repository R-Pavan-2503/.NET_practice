namespace StudentDemo
{
    public partial class Student
    {
        private string Name;
        private string Department;

        public void GetDetails()
        {
            Console.WriteLine("ENter the student name ; ");

            Name = Console.ReadLine();

            Console.WriteLine("Enter the student Deaprtment");
            Department = Console.ReadLine();
        }

        public partial void showMessage();

        public void Display()
        {
            Console.WriteLine("Student info");
            Console.WriteLine($"Name : {Name}");
            Console.WriteLine($"Department : {Department}");
        }
    }
}