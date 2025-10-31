using System;

namespace LibraryApi.Models
{
    public enum LoanStatus
    {
        CheckedOut,
        Returned
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

        public void Return() => Status = LoanStatus.Returned;
    }
}
