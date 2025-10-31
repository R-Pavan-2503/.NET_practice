using System;

namespace LibraryApi.Models
{
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
}
