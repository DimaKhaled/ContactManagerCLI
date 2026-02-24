using ContactManagerCLI.Models;
using ContactManagerCLI.Services;
using ContactManagerCLI.Storage;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.RegularExpressions;


namespace ContactManagerCLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== Welcome to Contact Manager CLI =====\n");

            string filePath = GetFilePath();
            IContactStorage contactStorage = new JsonContactStorage(filePath);
            IContactService contactService = new ContactService(contactStorage);
            IInputValidator inputValidator = new ContactValidator();
            
            DisplayContacts(contactService.ListContacts());

            int choice;
            do
            {
                DisplayMenu();

                choice = GetUserChoice();
                switch (choice)
                {
                    case 1:
                        AddContactMenu(contactService, inputValidator);
                        break;
                    case 2:
                        EditContactMenu(contactService, inputValidator);
                        break;
                    case 3:
                        DeleteContactMenu(contactService);
                        break;
                    case 4:
                        ViewContactMenu(contactService);
                        break;
                    case 5:
                        DisplayContacts(contactService.ListContacts());
                        break;
                    case 6:
                        SearchContactMenu(contactService);
                        break;
                    case 7:
                        FilterContactMenu(contactService);
                        break;
                    case 8:
                        contactService.Save();
                        break;
                    case 9:
                        Exit(contactService);
                        break;

                }
            } while (choice != 9);
            
        }
        


        static void Exit(IContactService contactService)
        {
            Console.WriteLine("\nDo you want to save to the file before exiting?");
            Console.WriteLine("1. Yes, save and exit");
            Console.WriteLine("2. No, just exit");
            int exitChoice;
            while (true)
            {
                string input = Console.ReadLine()!.Trim();
                if (int.TryParse(input, out exitChoice) && (exitChoice == 1 || exitChoice == 2))
                    break;
                Console.WriteLine("Invalid choice! Please enter 1 to save and exit or 2 to just exit:");
            }
            switch (exitChoice)
            {
                case 1:
                    contactService.Save();
                    break;
                case 2:
                    Console.WriteLine("Exiting without saving changes...");
                    break;
            }
            Console.WriteLine("Exiting application. Goodbye!");
        }


        static void FilterContactMenu(IContactService contactService)
        {
            Console.WriteLine("\nFilter by:");
            Console.WriteLine("1. Name");
            Console.WriteLine("2. Phone");
            Console.WriteLine("3. Email");
            Console.WriteLine("4. Creation Date");
            Console.WriteLine("Choose an option:");
            int filterChoice;
            string input = Console.ReadLine()!.Trim();
            while (true)
            {
                if (int.TryParse(input, out filterChoice) && filterChoice >= 1 && filterChoice <= 4)
                    break;
                Console.WriteLine("Invalid choice! Please enter a number between 1 and 4:");
                input = Console.ReadLine()!.Trim();
            }

            var results = new List<Contact>();
            switch (filterChoice)
            {
                case 1:
                    Console.WriteLine("Enter Name keyword");
                    string nameKeyword = Console.ReadLine()!.Trim();
                    results = contactService.Filter(c => c.Name.ToLower().Contains(nameKeyword.ToLower()));
                    break;
                case 2:
                    Console.WriteLine("Enter Phone keyword");
                    string phoneKeyword = Console.ReadLine()!.Trim();
                    results = contactService.Filter(c => c.Phone.Contains(phoneKeyword));
                    break;
                case 3:
                    Console.WriteLine("Enter Email keyword");
                    string emailKeyword = Console.ReadLine()!.Trim();
                    results = contactService.Filter(c => c.Email.ToLower().Contains(emailKeyword.ToLower()));
                    break;
                case 4:
                    Console.WriteLine("Enter Creation Date (YYYY-MM-DD)");
                    DateTime creationDate;
                    while (!DateTime.TryParse(Console.ReadLine(), out creationDate))
                    {
                        Console.WriteLine("Invalid date format! Please enter a valid date (YYYY-MM-DD):");
                    }
                    results = contactService.Filter(c => c.CreationDate.Date == creationDate.Date);
                    break;
            }
            DisplayContacts(results);
        }



        static void SearchContactMenu(IContactService contactService)
        {
            Console.WriteLine("\n===== Search Contact =====");
            Console.WriteLine("Enter keyword to search (Name/Phone/Email):");
            string keyword = Console.ReadLine()!.Trim();
            var results = contactService.SearchContacts(keyword);

            if (results.Count == 0)
            {
                Console.WriteLine("No contacts found matching the keyword!");
                return;
            }

            DisplayContacts(results);
        }



        static void ViewContactMenu(IContactService contactService)
        {
            Console.WriteLine("\n===== View Contact =====");
            Console.WriteLine("Enter contact Id to view:");
            int id;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out id))
                    break;
                Console.WriteLine("Invalid Id! Please enter a valid numeric Id:");
            }

            var c = contactService.ViewContact(id);
            if (c == null)
            {
                Console.WriteLine("Contact not found! Returning to main menu...");
                return;
            }

            Console.WriteLine(new string('-', 100));
            Console.WriteLine("{0, -5} | {1, -20} | {2, -15} | {3, -25} | {4, -10}", "ID", "Name", "Phone", "Email", "Creation Date");
            Console.WriteLine(new string('-', 100));

            Console.WriteLine("{0, -5} | {1, -20} | {2, -15} | {3, -25} | {4, -10}",
                                  c.Id, (c.Name.Length > 20 ? c.Name.Substring(0, 17) + "..." : c.Name),
                                  c.Phone, c.Email.Length > 25 ? c.Email.Substring(0, 22) + "..." : c.Email,
                                  c.CreationDate);
        }
        


        static void DeleteContactMenu(IContactService contactService)
        {
            Console.WriteLine("\n===== Delete Contact =====");
            Console.WriteLine("Enter contact Id to delete:");
            int id;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out id))
                    break;
                Console.WriteLine("Invalid Id! Please enter a valid numeric Id:");
            }

            if (contactService.DeleteContact(id))             
                Console.WriteLine("Contact deleted successfully!");
            else
                Console.WriteLine("Contact not found! Returning to main menu...");
        }



        static void EditContactMenu(IContactService contactService, IInputValidator validator)
        {
            Console.WriteLine("\n===== Edit Contact =====");
            Console.WriteLine("Enter contact Id to edit:");
            int id;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out id))
                    break;
                Console.WriteLine("Invalid Id! Please enter a valid numeric Id:");      
            }

            var contact = contactService.ViewContact(id);

            if (contact == null)
            {
                Console.WriteLine("Contact not found! Returning to main menu...");
                return;
            }

            Console.WriteLine("Enter new contact name: ");
            string name = Console.ReadLine()!.Trim();
            while (!validator.isValidName(name))
            {
                Console.WriteLine("Invalid name! Please enter a valid name:");
                name = Console.ReadLine()!.Trim();
            }

            Console.WriteLine("Enter new contact phone number:");
            string phone = Console.ReadLine()!.Trim();
            while (!validator.isValidPhone(phone))
            {
                Console.WriteLine("Invalid phone number! Please enter a valid phone number:");
                phone = Console.ReadLine()!.Trim();
            }

            Console.WriteLine("Enter new contact email:");
            string email = Console.ReadLine()!.Trim();
            while (!validator.isValidEmail(email))
            {
                Console.WriteLine("Invalid email! Please enter a valid email address:");
            }

            contactService.EditContact(id, name, phone, email);
            Console.WriteLine("Contact updated Successfully!\n");
        }



        static void AddContactMenu(IContactService contactService, IInputValidator validator)
        {
            Console.WriteLine("\n===== Add New Contact =====");

            Console.WriteLine("Enter contact name: ");
            string name = Console.ReadLine()!.Trim();
            while (!validator.isValidName(name))
            {
                Console.WriteLine("Invalid name! Please enter a valid name:");
                name = Console.ReadLine()!.Trim();
            }

            Console.WriteLine("Enter contact phone number:");
            string phone = Console.ReadLine()!.Trim();
            while (!validator.isValidPhone(phone))
            {
                Console.WriteLine("Invalid phone number! Please enter a valid phone number with 11 digits:");
                phone = Console.ReadLine()!.Trim();
            }

            Console.WriteLine("Enter contact email:");
            string email = Console.ReadLine()!.Trim();
            while (!validator.isValidEmail(email))
            {
                Console.WriteLine("Invalid email! Please enter a valid email address (e.g jack@test.com):");
                email = Console.ReadLine()!.Trim();
            }

            contactService.AddContact(name, phone, email);
            Console.WriteLine("Contact added successfully!");
        }



        static int GetUserChoice()
        {
            int choice;
            while (true)
            {
                string input = Console.ReadLine()!.Trim();
                if (int.TryParse(input, out choice) && choice >= 1 && choice <= 9)
                {
                    return choice;                   
                }
                Console.WriteLine("Invalid choice! Please enter a number between 1 and 9:");
            }
        }



        static void DisplayMenu()
        {
            Console.WriteLine("\n===== Contact Manager Main Menu =====");
            Console.WriteLine("[1] Add Contact");
            Console.WriteLine("[2] Edit Contact");
            Console.WriteLine("[3] Delete Contact");
            Console.WriteLine("[4] View Contact");
            Console.WriteLine("[5] List Contacts");
            Console.WriteLine("[6] Search");
            Console.WriteLine("[7] Filter");
            Console.WriteLine("[8] Save");
            Console.WriteLine("[9] Exit");
            Console.WriteLine("Choose an option:");
        }



        static void DisplayContacts(List<Contact> contacts)
        {
            if (contacts == null || contacts.Count == 0)
            {
                Console.WriteLine("-> No contacts found.\n");
                return;
            }
            Console.WriteLine("-> Existing contacts:");

            Console.WriteLine(new string('-', 100));
            Console.WriteLine("{0, -5} | {1, -20} | {2, -15} | {3, -25} | {4, -10}", "ID", "Name", "Phone", "Email", "Creation Date");
            Console.WriteLine(new string('-', 100));

            foreach (var c in contacts)
            {
                Console.WriteLine("{0, -5} | {1, -20} | {2, -15} | {3, -25} | {4, -10}",
                                  c.Id, (c.Name.Length > 20 ? c.Name.Substring(0, 17) + "..." : c.Name),
                                  c.Phone, c.Email.Length > 25 ? c.Email.Substring(0, 22) + "..." : c.Email,
                                  c.CreationDate);
            }
            Console.WriteLine(new string('-', 100));
        }



        static string GetFilePath()
        {
            string path;
            
            do
            {
                Console.WriteLine("Enter JSON file path to store contacts:");
                path = Console.ReadLine()!.Trim();

                if (string.IsNullOrWhiteSpace(path))
                {
                    Console.WriteLine("Path cannot be empty! TRY AGAIN");
                    continue;
                }

                if (Path.GetExtension(path).ToLower() != ".json")
                {
                    Console.WriteLine("File must have .json extension! TRY AGAIN");
                    continue;
                }

                string directory = Path.GetDirectoryName(path);
                if (string.IsNullOrWhiteSpace(directory))
                {
                    directory = Directory.GetCurrentDirectory();
                    path = Path.Combine(directory, path);
                }

                if (!Directory.Exists(directory))
                {
                    Console.WriteLine("Directory doesn't exist! Try again or create the folder first");
                    continue;
                }

                string fileName = Path.GetFileName(path);
                if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    Console.WriteLine("File name contains invalid characters! TRY AGAIN");
                    continue;
                }

                break;
            } while (true);

            return path;
        }
    }
}
