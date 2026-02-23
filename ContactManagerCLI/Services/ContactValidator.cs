using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ContactManagerCLI.Services
{
    internal class ContactValidator : IInputValidator
    {
        public bool isValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }


        public bool isValidPhone(string phone)
        {
            return !string.IsNullOrWhiteSpace(phone) && phone.All(char.IsDigit) && phone.Length == 11;
        }


        public bool isValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
