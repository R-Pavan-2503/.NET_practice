namespace StudentGrade
{
    class Program
    {
        static void Main()
        {
            double Average = 0;

            for (int i = 0; i < 5; i++)
            {
                int CurStudentMark = 0;
                bool ValidMark = false;

                while (!ValidMark)
                {
                    Console.WriteLine($"ENter the mark for student {i + 1}:");
                    CurStudentMark = Convert.ToInt32(Console.ReadLine());

                    if (CurStudentMark >= 0 && CurStudentMark <= 500)
                    {
                        ValidMark = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Marks must be between 0 and 500.");
                    }
                }


                double CurStudentPercentage = (double)CurStudentMark / 500 * 100;
                string CurStudentGrade = "";


                if (CurStudentPercentage >= 90 && CurStudentPercentage <= 100)
                {
                    CurStudentGrade = "A+";
                }
                else if (CurStudentPercentage >= 80 && CurStudentPercentage < 90)
                {
                    CurStudentGrade = "A";
                }
                else if (CurStudentPercentage >= 70 && CurStudentPercentage < 80)
                {
                    CurStudentGrade = "B";
                }
                else if (CurStudentPercentage >= 60 && CurStudentPercentage < 70)
                {
                    CurStudentGrade = "C";
                }
                else
                {
                    CurStudentGrade = "Fail";
                }


                Console.WriteLine($"Student {i + 1}: Marks = {CurStudentMark}, Percentage = {CurStudentPercentage}%, Grade = {CurStudentGrade}");


                Average += CurStudentMark;
            }


            Average /= 5;
            Console.WriteLine($"The average mark for all 5 students is {Average}");
        }
    }
}
