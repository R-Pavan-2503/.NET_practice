using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApi.Models
{
    public class Library
    {
        private Dictionary<string, Book> _Books { get; set; }
        private Dictionary<int, Patron> _Patrons { get; set; }
        private List<Loan> _Loans { get; set; }

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
            {
                _Patrons.Add(patron.Id, patron);
            }
        }

        public List<Book> GetAllBooks()
        {
            return _Books.Values.ToList();
        }

        public List<Patron> GetAllPatrons()
        {
            return _Patrons.Values.ToList();
        }

        public Patron? FindPatronById(int id)
        {
            if (_Patrons.ContainsKey(id))
                return _Patrons[id];
            return null;
        }
        public Book? FindBookById(string isbn)
        {
            if (_Books.ContainsKey(isbn))
                return _Books[isbn];
            return null;
        }

        public string CheckOutBook(int patronId, string isbn)
        {
            var patron = FindPatronById(patronId);
            if (patron == null)
            {
                return "Patron not found.";
            }

            var book = FindBookById(isbn);
            if (book == null)
                return "Book not found.";

            if (book.CheckOut())
            {

                patron.AddBookToPatron(book);

                var loan = new Loan(book, patron);
                _Loans.Add(loan);
                return $"Book '{book.Title}' checked out successfully by {patron.Name}.";
            }
            else
            {


                return "Book is not available (all copies are already checked out).";
            }
        }


        public string ReturnBook(int patronId, string isbn)
        {
            var patron = FindPatronById(patronId);
            if (patron == null)
            {
                return "Patron not found.";
            }

            var book = FindBookById(isbn);
            if (book == null)
            {
                return "Book not found.";
            }

            var loan = _Loans.FirstOrDefault(l =>
        l.Book.ISBN == isbn &&
        l.Patron.Id == patronId &&
        l.Status == LoanStatus.CheckedOut);

            if (loan == null)
            {
                return $"No active loan found for '{book.Title}' by {patron.Name}.";
            }

            book.ReturnBook();

            patron.RemoveBookFromPatron(book);

            loan.Return();

            return $"Book '{book.Title}' returned successfully by {patron.Name}.";
        }

    }
}
