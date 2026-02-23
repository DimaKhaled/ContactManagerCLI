using ContactManagerCLI.Models;
using ContactManagerCLI.Storage;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ContactManagerCLI.Storage
{
    internal class JsonContactStorage : IContactStorage
    {
        private readonly string _filePath;


        public JsonContactStorage(string filePath)
        {
            _filePath = filePath;
        }


        public List<Contact> LoadContacts()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new List<Contact>();

                string json = File.ReadAllText(_filePath);

                if (string.IsNullOrWhiteSpace(json))
                    return new List<Contact>();

                return JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error happened while loading contacts: " + e.Message);
                return new List<Contact>();
            }
        }


        public void SaveContacts(List<Contact> contacts)
        {
            try
            {
                string json = JsonSerializer.Serialize(contacts, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error happened while saving contacts: " + e.Message);
            }
        }
    }
}
