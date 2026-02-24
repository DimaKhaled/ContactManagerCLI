using System;
using System.Collections.Generic;
using System.Text;
using ContactManagerCLI.Models;

namespace ContactManagerCLI.Services
{
    internal interface IContactService
    {
        void AddContact(string name, string phone, string email);
        void EditContact(int id, string name, string phone, string email);
        bool DeleteContact(int id);
        Contact? ViewContact(int id);
        List<Contact> ListContacts();
        List<Contact> SearchContacts(string keyword);
        List<Contact> Filter(Func<Contact, bool> filter);
        void Save();
    }
}
