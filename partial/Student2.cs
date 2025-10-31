namespace StudentDemo
{
    public partial class Student
    {
        private string ClassSection;
        private string Year;

        public void GetDetails1()
        {
            Console.WriteLine("ENter the student class Section ; ");

            ClassSection = Console.ReadLine();

            Console.WriteLine("Enter the student Year");
            Year = Console.ReadLine();
        }

        public partial void showMessage()
        {
            Console.WriteLine("Welcome to students portal");
        }

        public void Display1()
        {
            Console.WriteLine("Student info");
            Console.WriteLine($"ClassSection : {ClassSection}");
            Console.WriteLine($"year : {Year}");
        }
    }
}