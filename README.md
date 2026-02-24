# ContactManagerCLI

A simple **Command-Line Contact Management System** built in C#.  
This application allows users to **add, edit, delete, view, list, search, filter, and save contacts** using a JSON file as storage.  

It demonstrates **Object-Oriented Programming (OOP)**, **SOLID principles**.

---

## Table of Contents

- [Features](#features)  
- [Project Structure](#project-structure)  
- [How It Works](#how-it-works)  
- [How to Run](#how-to-run)  
- [Validation & Behavior](#validation--behavior)  
- [Test Cases](#test-cases)  
- [Future Improvements](#future-improvements)  

---

## Features

- **Add Contact** with auto-generated ID and creation date  
- **Edit Contact** by ID  
- **Delete Contact** by ID  
- **View Contact** details  
- **List Contacts** in an organized table  
- **Search Contacts** by name, phone, or email  
- **Filter Contacts** with multiple criteria (delegate-based)  
- **Save Contacts** to JSON file  
- **Load Contacts** on startup  

---

## Project Structure

```
ContactManagerCLI
│
├── Models
│ └── Contact.cs              # Contact class with Id, Name, Phone, Email, CreationDate
│
├── Services
│ ├── IContactService.cs      # Interface defining contact operations
│ ├── IInputValidator.cs      # Interface for input validation
│ ├── ContactService.cs       # Implements contact management logic
│ └── ContactValidator.cs     # Implements input validation logic
│
├── Storage
│ ├── IContactStorage.cs      # Interface for storage operations
│ └── JsonContactStorage.cs   # Implements JSON-based storage
│
└── Program.cs                # Main CLI entry point and menu
```

---

## How It Works

1. **Startup:**  
   - Prompts user for JSON file path to store contacts.  
   - Loads existing contacts if file exists, otherwise creates an empty JSON array.  

2. **Main Menu:**  

    1. Add Contact

    2. Edit Contact

    3. Delete Contact

    4. View Contact

    5. List Contacts

    6. Search

    7. Filter

    8. Save

    9. Exit


3. **Flow:**  
- Users perform operations interactively.  
- Contacts are stored in memory.  
- `Save` must be selected to persist changes to the JSON file.  
- On restart, contacts are loaded from the JSON file.  

4. **Validation:**  
- Name cannot be empty or numeric  
- Phone must be **11 digits**  
- Email must follow standard email format  
- File path must be valid and point to a `.json` file  

---

## How to Run

Follow these steps to run the ContactManagerCLI application:

1. **Clone the Repository**  
   If you haven’t already, clone the repository to your local machine:
   ```bash
   git clone https://github.com/your-username/ContactManagerCLI.git
 Replace your-username with your GitHub username.

2. **Open the Project in Visual Studio**
- Navigate to the project folder ContactManagerCLI
- Open the solution file ContactManagerCLI.sln in Visual Studio 2022 (or newer)

3. **Restore NuGet Packages (if needed)**
- Go to Tools → NuGet Package Manager → Manage NuGet Packages for Solution
- Click Restore to ensure all packages are installed

4. **Build the Project**
- Press Ctrl + Shift + B or go to Build → Build Solution
- Ensure there are no build errors

5. **Run the Application**
- Press F5 (Start with Debugging) or Ctrl + F5 (Start without Debugging)
- The console window will appear

---

## Validation & Behavior

- Invalid input will **prompt the user again** until valid  
- If a contact ID is not found for Edit/Delete/View, user is informed  
- `List Contacts` displays **ID, Name, Phone, Email, Creation Date** in aligned columns  
- `Search` is **case-insensitive** and matches partial names/phones/emails  
- `Filter` allows **multiple criteria** using a delegate  
- Saving updates the JSON file; unsaved changes are lost if program exits  

---

## Test Cases

1. **Startup & File Path:**  
- Empty input → prompts again  
- Invalid characters → prompts again  
- Wrong extension → prompts again  
- Non-existing directory → prompts again  

2. **Add Contact:**  
- Invalid name → loop until valid  
- Invalid phone → loop until valid  
- Invalid email → loop until valid  
- Valid input → contact added with ID & Creation Date  

3. **Edit/Delete/View:**  
- Valid ID → operation succeeds  
- Invalid ID → shows "Contact not found"  

4. **Search & Filter:**  
- Partial matches allowed  
- Case-insensitive  
- Multiple filter criteria supported  

5. **Persistence:**  
- Save then restart → contacts loaded from JSON  
- Exit without saving → changes not persisted  

6. **Edge Cases:**  
- Very long names or emails handled gracefully  
- Corrupted JSON → program catches exception and continues with empty list  

---

## Future Improvements

- Add **undo/redo functionality**  
- Allow **sorting by different fields**  
- Implement **unit tests** for automated verification  
- Add **export/import** of contacts in CSV  

---

