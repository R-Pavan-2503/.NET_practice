namespace ATM
{
    class Program
    {
        static void Main()
        {

            Console.WriteLine("Enter the costomer name : ");
            string CusName = Console.ReadLine();

            double CusBalance = 0.0;

            int CusChoice = 0;
            while (CusChoice != 4)
            {
                Console.WriteLine("Hello " + CusName);

                Console.WriteLine("Choose the Operation You need to do :");
                Console.WriteLine("1. Deposit");
                Console.WriteLine("2. Withdraw");
                Console.WriteLine("3. Check Balance");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice: ");
                CusChoice = Convert.ToInt32(Console.ReadLine());


                switch (CusChoice)
                {
                    case 1:
                        Console.Write("Enter deposit amount: ");
                        double deposit = Convert.ToDouble(Console.ReadLine());
                        CusBalance += deposit;
                        Console.WriteLine($"You have successfully deposited {deposit:C}. New balance: {CusBalance}");
                        break;

                    case 2:
                        Console.Write("Enter withdrawal amount: ");
                        double withdraw = Convert.ToDouble(Console.ReadLine());


                        if (withdraw <= CusBalance)
                        {
                            CusBalance -= withdraw;
                            Console.WriteLine($"You have withdrawn {withdraw}. Remaining balance: {CusBalance}");
                        }
                        else
                        {
                            Console.WriteLine("Insufficient balance");
                        }
                        break;

                    case 3:
                        Console.WriteLine($"Your current balance is: {CusBalance}");
                        break;

                    case 4:
                        Console.WriteLine($"Thank you, {CusName}! ");
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }


            }

        }
    }

}