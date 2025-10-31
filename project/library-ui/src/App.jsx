import React, { useState, useEffect } from "react";
import axios from "axios";


const API_URL = "http://localhost:5282"

function App() {
  const [books, setBooks] = useState([]);
  const [patronId, setPatronId] = useState("");
  const [isbn, setIsbn] = useState("");
  const [message, setMessage] = useState("");




  const fetchBooks = async () => {
    try {
      const response = await axios.get(`${API_URL}/api/books`);
      setBooks(response.data);
    } catch (error) {
      console.error("Error fetching books:", error);
      setMessage("Failed to load books. Check API connection.");
    }
  };


  useEffect(() => {
    fetchBooks();
  }, []);




  const handleCheckout = async () => {
    try {
      const response = await axios.post(`${API_URL}/api/library/checkout`, {
        patronId: parseInt(patronId),
        isbn: isbn,
      });
      setMessage(response.data.message);
      fetchBooks();
    } catch (error) {
      if (error.response) {
        setMessage(error.response.data.message);
      } else {
        setMessage("Error checking out book.");
      }
    }
  };




  const handleReturn = async () => {
    try {
      const response = await axios.post(`${API_URL}/api/library/return`, {
        patronId: parseInt(patronId),
        isbn: isbn,
      });
      setMessage(response.data.message);
      fetchBooks();
    } catch (error) {
      if (error.response) {
        setMessage(error.response.data.message);
      } else {
        setMessage("Error returning book.");
      }
    }
  };




  return (
    <div style={{ padding: "2rem", fontFamily: "Arial" }}>
      <h1>📚 Library Catalog</h1>

      <h2>Available Books</h2>
      <ul>
        {books.map((book) => (
          <li key={book.isbn}>
            <strong>{book.title}</strong> — ISBN: {book.isbn} <br />
            (Available: {book.availableQuantity} / {book.totalQuantity})
          </li>
        ))}
      </ul>

      <hr />
      <h2>Book Checkout / Return</h2>
      <div style={{ marginBottom: "1rem" }}>
        <label>Patron ID: </label>
        <input
          type="number"
          value={patronId}
          onChange={(e) => setPatronId(e.target.value)}
          style={{ marginRight: "1rem" }}
        />
        <label>ISBN: </label>
        <input
          type="text"
          value={isbn}
          onChange={(e) => setIsbn(e.target.value)}
          style={{ marginRight: "1rem" }}
        />
      </div>

      <button
        onClick={handleCheckout}
        style={{ marginRight: "1rem", padding: "0.5rem 1rem" }}
      >
        Check Out Book
      </button>

      <button onClick={handleReturn} style={{ padding: "0.5rem 1rem" }}>
        Return Book
      </button>

      {message && (
        <p style={{ marginTop: "1rem", fontWeight: "bold", color: "green" }}>
          {message}
        </p>
      )}
    </div>
  );
}

export default App;
