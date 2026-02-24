using ContactManagerCLI.Models;
using ContactManagerCLI.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactManagerCLI.Services
{
    internal class ContactService : IContactService
    {
        private readonly IContactStorage _contactStorage;
        private readonly List<Contact> _contacts;

        public ContactService(IContactStorage contactStorage)
        {
            _contactStorage = contactStorage;
            _contacts = _contactStorage.LoadContacts();
        }


        public void AddContact(string name, string phone, string email)
        {
            int newId = _contacts.Count() > 0 ? _contacts.Max(c => c.Id) + 1 : 1;
            var newContact = new Contact
            {
                Id = newId,
                Name = name,
                Phone = phone,
                Email = email,
                CreationDate = DateTime.Now
            };
            _contacts.Add(newContact);
        }

        public void EditContact(int id, string name, string phone, string email)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            contact.Name = name;
            contact.Phone = phone;
            contact.Email = email;
        }

        public bool DeleteContact(int id)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
                return false;

            _contacts.Remove(contact);
            return true;
        }

        public Contact? ViewContact(int id)
        {
            return _contacts.FirstOrDefault(c => c.Id == id);
        }

        public List<Contact> ListContacts()
        {
            return _contacts.OrderBy(c => c.Name).ToList();
        }

        public List<Contact> SearchContacts(string keyword)
        {
            keyword = keyword.ToLower();

            return _contacts.Where(c => c.Name.ToLower().Contains(keyword) ||
                                   c.Phone.Contains(keyword) ||
                                   c.Email.ToLower().Contains(keyword)).ToList(); 
        }

        public List<Contact> Filter(Func<Contact, bool> filter)
        {
            return _contacts.Where(filter).ToList();
        }

        public void Save()
        {
            _contactStorage.SaveContacts(_contacts);
            Console.WriteLine("Contacts saved successfuly");
        }
    }
}
