using System;
using System.Collections.Generic;
using System.Text;

namespace ContactManagerCLI.Models
{
    internal class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
