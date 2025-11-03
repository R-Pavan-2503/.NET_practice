import React, { useState, useEffect } from 'react';
import EmployeeForm from './EmployeeForm'; // 1. Import the new form
import './App.css';

// Make sure this is your API's correct URL
const API_URL = "http://localhost:5029/api/employees";

function App() {
  const [employees, setEmployees] = new useState([]);
  const [error, setError] = new useState(null);

  // 2. Renamed this to be more reusable
  const fetchEmployees = () => {
    fetch(API_URL)
      .then(response => response.json())
      .then(data => setEmployees(data))
      .catch(error => console.error('Error fetching data:', error));
  };

  // 3. This 'useEffect' hook runs once when the component loads
  useEffect(() => {
    fetchEmployees();
  }, []); // The empty array [] means "run this effect only once"

  // 4. This function will be called by EmployeeForm
  const handleEmployeeAdded = (newEmployee) => {
    setError(null); // Clear any previous errors

    fetch(API_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(newEmployee), // Convert the JS object to a JSON string
    })
      .then(response => {
        if (!response.ok) {
          // If the server sends an error (like 409 Conflict), handle it
          return response.json().then(err => { throw new Error(err.message || "Could not add employee") });
        }
        return response.json(); // Get the new employee back from the API (with ID)
      })
      .then(createdEmployee => {
        // Add the new employee to our state to update the list
        setEmployees([...employees, createdEmployee]);
      })
      .catch(error => {
        // Handle errors, like the duplicate email
        console.error('Error adding employee:', error);
        setError(error.message); // Show the error message on the page
      });
  };

  return (
    <div className="App">
      <header className="App-header">
        <h1>Employee Management</h1>

        {/* 5. Render the new form and pass the function as a prop */}
        <EmployeeForm onEmployeeAdded={handleEmployeeAdded} />

        {/* 6. Display the error message if one exists */}
        {error && <p className="error-message">{error}</p>}

        <hr />

        <h2>Current Employees</h2>
        <div className="employee-list">
          {employees.length > 0 ? (
            <ul>
              {employees.map(employee => (
                <li key={employee.id}>
                  {employee.firstName} {employee.lastName} ({employee.position})
                </li>
              ))}
            </ul>
          ) : (
            <p>No employees found.</p>
          )}
        </div>
      </header>
    </div>
  );
}

export default App;