using System.Reflection.Metadata;

namespace bankTrain
{
    interface IBankAccount
    {
        void Deposit(double amount);

        void withdraw(double amount);

    }

    class SavingAccount : IBankAccount
    {
        private double balance = 0;

        public void Deposit(double amount)
        {
            balance += amount;
            Console.WriteLine($"Depeositied : {amount}");
            Console.WriteLine($"current Balance : {balance}");
        }

        public void withdraw(double amount)
        {
            if (amount <= balance)
            {
                balance -= amount;
                Console.WriteLine($"Withdrwas : {amount}");
                Console.WriteLine($"current balance : {balance}");
            }
            else
            {
                Console.WriteLine("Insufficient Balance");
            }
        }


    }

    class Program
    {
        static void Main()
        {
            IBankAccount bankAccount = new SavingAccount();

            Console.WriteLine("Enter the amount :");

            double amount = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Eter the amount for withdrasing");

            double withdrawAmount = Convert.ToDouble(Console.ReadLine());

            bankAccount.Deposit(amount);
            bankAccount.withdraw(withdrawAmount);
        }
    }
}