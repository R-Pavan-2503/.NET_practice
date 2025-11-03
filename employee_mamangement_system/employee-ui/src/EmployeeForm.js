import React, { useState } from 'react';

function EmployeeForm({ onEmployeeAdded }) {
    // State for each input field
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [position, setPosition] = useState('');
    const [email, setEmail] = useState('');

    const handleSubmit = (e) => {
        // Prevent the browser from refreshing the page
        e.preventDefault();

        // Create the new employee object
        const newEmployee = {
            firstName,
            lastName,
            position,
            email,
        };

        // Call the function passed down from App.js
        onEmployeeAdded(newEmployee);

        // Clear the form fields
        setFirstName('');
        setLastName('');
        setPosition('');
        setEmail('');
    };

    return (
        <form className="employee-form" onSubmit={handleSubmit}>
            <h3>Add New Employee</h3>
            <div>
                <label>First Name:</label>
                <input
                    type="text"
                    value={firstName}
                    onChange={(e) => setFirstName(e.target.value)}
                    required
                />
            </div>
            <div>
                <label>Last Name:</label>
                <input
                    type="text"
                    value={lastName}
                    onChange={(e) => setLastName(e.target.value)}
                    required
                />
            </div>
            <div>
                <label>Position:</label>
                <input
                    type="text"
                    value={position}
                    onChange={(e) => setPosition(e.target.value)}
                    required
                />
            </div>
            <div>
                <label>Email:</label>
                <input
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />
            </div>
            <button type="submit">Add Employee</button>
        </form>
    );
}

export default EmployeeForm;