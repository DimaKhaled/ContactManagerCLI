using System;
using System.Collections.Generic;
using System.Text;

namespace ContactManagerCLI.Services
{
    internal interface IInputValidator
    {
        bool isValidName(string name);
        bool isValidEmail(string email);
        bool isValidPhone(string phone);
    }
}
