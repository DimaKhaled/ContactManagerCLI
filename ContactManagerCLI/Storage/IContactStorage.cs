using System.Collections.Generic;
using ContactManagerCLI.Models;

namespace ContactManagerCLI.Storage
{
    internal interface IContactStorage
    {
        List<Contact> LoadContacts();
        void SaveContacts(List<Contact> contacts);
    }
}
