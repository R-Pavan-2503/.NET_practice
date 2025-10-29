using System;
using System.Collections.Generic;
using System.Linq;

namespace BankAccountManagementSystem
{
    public class Bank
    {
        private List<Customer> _customers;
        private List<Account> _accounts;

        public Bank()
        {
            _customers = new List<Customer>();
            _accounts = new List<Account>();
        }




        public Customer CreateCustomer(int id, string firstName, string lastName)
        {
            var customer = new Customer(id, firstName, lastName);
            _customers.Add(customer);
            Console.WriteLine($"Customer created: {firstName} {lastName} (ID: {id})");
            return customer;
        }




        public Account CreateAccount(Customer customer, string accountNumber)
        {
            var account = new Account(accountNumber, customer);
            _accounts.Add(account);
            Console.WriteLine($"Account created for {customer.FirstName} {customer.LastName} (Account Number: {accountNumber})");
            return account;
        }




        public void ListCustomers()
        {
            Console.WriteLine("\n--- List of Customers ---");
            foreach (var c in _customers)
            {
                Console.WriteLine($"ID: {c.Id} | Name: {c.FirstName} {c.LastName}");
            }
        }




        public void ListAccounts()
        {
            Console.WriteLine("\n--- List of Accounts ---");
            foreach (var a in _accounts)
            {
                Console.WriteLine($"Account: {a.AccountNumber} | Customer: {a.Customer.FirstName} {a.Customer.LastName} | Balance: {a.Balance}");
            }
        }




        private Account? FindAccount(string accountNumber)
        {
            return _accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
        }




        public void Transfer(string fromAccountNum, string toAccountNum, decimal amount)
        {
            var fromAccount = FindAccount(fromAccountNum);
            if (fromAccount == null)
            {
                Console.WriteLine("Invalid 'from' account number");
                return;
            }

            var toAccount = FindAccount(toAccountNum);
            if (toAccount == null)
            {
                Console.WriteLine("Invalid 'to' account number");
                return;
            }

            bool withdrawStatus = fromAccount.Withdraw(amount);
            if (withdrawStatus)
            {
                toAccount.Deposit(amount);
                Console.WriteLine($"Transfer of {amount:C} from {fromAccountNum} to {toAccountNum} successful!");
            }
            else
            {
                Console.WriteLine("Transfer failed during withdrawal.");
            }
        }
    }




    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Customer(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }
    }




    public class Account
    {
        public string AccountNumber { get; set; }
        public Customer Customer { get; set; }
        public decimal Balance { get; private set; }

        private List<Transaction> _transactions;

        public Account(string accountNumber, Customer customer)
        {
            AccountNumber = accountNumber;
            Customer = customer;
            Balance = 0;
            _transactions = new List<Transaction>();
        }


        public bool Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("You can't deposit a negative amount or 0");
                var currTransaction = new Transaction(DateTime.Now, "Deposit failed: amount <= 0", amount, Balance);
                _transactions.Add(currTransaction);
                return false;
            }

            Balance += amount;
            Console.WriteLine($"{amount:C} has been successfully added to the account");
            var successTransaction = new Transaction(DateTime.Now, "Deposit successful", amount, Balance);
            _transactions.Add(successTransaction);
            return true;
        }


        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("You can't withdraw a negative amount or 0");
                var currTransaction = new Transaction(DateTime.Now, "Withdraw failed: amount <= 0", amount, Balance);
                _transactions.Add(currTransaction);
                return false;
            }

            if (amount > Balance)
            {
                Console.WriteLine("You can't withdraw more than you have");
                var currTransaction = new Transaction(DateTime.Now, "Withdraw failed: insufficient funds", amount, Balance);
                _transactions.Add(currTransaction);
                return false;
            }

            Balance -= amount;
            Console.WriteLine($"{amount:C} has been successfully withdrawn from the account");
            var successTransaction = new Transaction(DateTime.Now, "Withdraw successful", amount, Balance);
            _transactions.Add(successTransaction);
            return true;
        }


        public void PrintTransactionHistory()
        {
            Console.WriteLine($"\nTransaction history for account {AccountNumber}:");
            foreach (var t in _transactions)
            {
                Console.WriteLine($"{t.DateTime} | {t.Description} | Amount: {t.Amount:C} | Balance: {t.ResultingBalance:C}");
            }
        }
    }




    public class Transaction
    {
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal ResultingBalance { get; set; }

        public Transaction(DateTime datetime, string description, decimal amount, decimal balance)
        {
            DateTime = datetime;
            Description = description;
            Amount = amount;
            ResultingBalance = balance;
        }
    }




    public class Program
    {
        public static void Main()
        {
            var bank = new Bank();


            var alice = bank.CreateCustomer(1, "Alice", "Johnson");
            var bob = bank.CreateCustomer(2, "Bob", "Williams");


            var aliceAccount = bank.CreateAccount(alice, "A1001");
            var bobAccount = bank.CreateAccount(bob, "A1002");


            aliceAccount.Deposit(2000);
            aliceAccount.Withdraw(300);
            bank.Transfer("A1001", "A1002", 500);
            bank.Transfer("A1001", "A9999", 100);


            aliceAccount.PrintTransactionHistory();
            bobAccount.PrintTransactionHistory();


            bank.ListCustomers();
            bank.ListAccounts();
        }
    }
}
