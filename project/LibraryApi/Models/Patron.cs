using System.Collections.Generic;

namespace LibraryApi.Models
{
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

        public void AddBookToPatron(Book book) => _Books.Add(book);
        public void RemoveBookFromPatron(Book book) => _Books.Remove(book);
    }
}
