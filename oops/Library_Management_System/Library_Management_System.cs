using System;
using System.Collections.Generic;
using System.Linq;

namespace Library_Management_System
{
    public enum LoanStatus
    {
        CheckedOut,
        Returned
    }

    public class Book
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }

        public Book(string isbn, string title, int totalQuantity)
        {
            ISBN = isbn;
            Title = title;
            TotalQuantity = totalQuantity;
            AvailableQuantity = totalQuantity;
        }

        public bool CheckOut()
        {
            if (AvailableQuantity > 0)
            {
                AvailableQuantity--;
                return true;
            }
            return false;
        }

        public void ReturnBook()
        {
            if (AvailableQuantity < TotalQuantity)
            {
                AvailableQuantity++;
            }
        }
    }

    public class Patron
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private List<Book> _Books { get; set; }

        public Patron(int id, string name)
        {
            Id = id;
            Name = name;
            _Books = new List<Book>();
        }

        public void AddBookToPatron(Book book)
        {
            _Books.Add(book);
        }

        public void RemoveBookFromPatron(Book book)
        {
            _Books.Remove(book);
        }

        public void PrintBorrowedBooks()
        {
            if (_Books.Count == 0)
            {
                Console.WriteLine($"{Name} has not borrowed any books.");
                return;
            }

            Console.WriteLine($"{Name}'s borrowed books:");
            foreach (var book in _Books)
            {
                Console.WriteLine($"- {book.Title}");
            }
        }
    }

    public class Loan
    {
        public Book Book { get; private set; }
        public Patron Patron { get; private set; }
        public DateTime DueDate { get; set; }
        public LoanStatus Status { get; private set; }

        public Loan(Book book, Patron patron)
        {
            Book = book;
            Patron = patron;
            DueDate = DateTime.Now.AddDays(14);
            Status = LoanStatus.CheckedOut;
        }

        public void Return()
        {
            Status = LoanStatus.Returned;
        }
    }

    public class Library
    {
        private Dictionary<string, Book> _Books { get; set; }
        private Dictionary<int, Patron> _Patrons { get; set; }
        private List<Loan> _Loans { get; set; }

        public delegate void OverdueBookHandler(Loan loan);
        public event OverdueBookHandler OnBookOverdue;

        public Library()
        {
            _Books = new Dictionary<string, Book>();
            _Patrons = new Dictionary<int, Patron>();
            _Loans = new List<Loan>();
        }

        public void RegisterBook(Book book)
        {
            if (!_Books.ContainsKey(book.ISBN))
                _Books.Add(book.ISBN, book);
        }

        public void RegisterPatron(Patron patron)
        {
            if (!_Patrons.ContainsKey(patron.Id))
                _Patrons.Add(patron.Id, patron);
        }

        public Book? FindBook(string isbn)
        {
            return _Books.ContainsKey(isbn) ? _Books[isbn] : null;
        }

        public Patron? FindPatron(int patronId)
        {
            return _Patrons.ContainsKey(patronId) ? _Patrons[patronId] : null;
        }

        public void CheckOutBook(int patronId, string isbn)
        {
            var patron = FindPatron(patronId);
            var book = FindBook(isbn);

            if (patron == null)
            {
                Console.WriteLine($"Error: Patron with ID {patronId} not found.");
                return;
            }

            if (book == null)
            {
                Console.WriteLine($"Error: Book with ISBN {isbn} not found.");
                return;
            }

            if (book.CheckOut())
            {
                patron.AddBookToPatron(book);
                var loan = new Loan(book, patron);
                _Loans.Add(loan);
                Console.WriteLine($"Success: {patron.Name} checked out '{book.Title}'. Due: {loan.DueDate.ToShortDateString()}");
            }
            else
            {
                Console.WriteLine($"Sorry, '{book.Title}' is not available right now.");
            }
        }

        public void ReturnBook(int patronId, string isbn)
        {
            var patron = FindPatron(patronId);
            var book = FindBook(isbn);

            if (patron == null || book == null)
            {
                Console.WriteLine("Error: Invalid patron or book.");
                return;
            }

            var loan = _Loans.FirstOrDefault(l => l.Book.ISBN == isbn && l.Patron.Id == patronId && l.Status == LoanStatus.CheckedOut);

            if (loan == null)
            {
                Console.WriteLine($"Error: No active loan found for '{book.Title}' by {patron.Name}.");
                return;
            }

            book.ReturnBook();
            patron.RemoveBookFromPatron(book);
            loan.Return();

            Console.WriteLine($"Return successful: '{book.Title}' returned by {patron.Name}.");
        }

        public void CheckForOverdueBooks()
        {
            foreach (var loan in _Loans)
            {
                if (loan.Status == LoanStatus.CheckedOut && loan.DueDate < DateTime.Now)
                {
                    OnBookOverdue?.Invoke(loan);
                }
            }
        }

        public List<Loan> GetAllLoans()
        {
            return _Loans;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            
            Library library = new Library();

            
            void NotifyOverdue(Loan loan)
            {
                Console.WriteLine($"WARNING: '{loan.Book.Title}' borrowed by {loan.Patron.Name} is OVERDUE! (Due on {loan.DueDate.ToShortDateString()})");
            }

            
            library.OnBookOverdue += NotifyOverdue;

            
            var book1 = new Book("111", "The C# Pro", 1);
            var book2 = new Book("222", "Learning OOP", 5);

            var patron1 = new Patron(1, "Alice");
            var patron2 = new Patron(2, "Bob");

            library.RegisterBook(book1);
            library.RegisterBook(book2);
            library.RegisterPatron(patron1);
            library.RegisterPatron(patron2);

            Console.WriteLine("\n=== Simulation Start ===\n");

            
            library.CheckOutBook(1, "222"); 
            library.CheckOutBook(2, "111"); 
            library.CheckOutBook(1, "111"); 
            library.ReturnBook(1, "222");   

            
            library.CheckOutBook(2, "222"); 

            
            var overdueLoan = library
                .GetAllLoans()
                .FirstOrDefault(l => l.Patron.Id == 2 && l.Book.ISBN == "222");

            if (overdueLoan != null)
            {
                
                overdueLoan.DueDate = DateTime.Now.AddDays(-10);
            }

            Console.WriteLine("\n=== Checking for Overdue Books ===\n");

            
            library.CheckForOverdueBooks();

            Console.WriteLine("\n=== Simulation Complete ===");
        }
    }
}
