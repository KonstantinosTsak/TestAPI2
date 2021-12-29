using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestAPI2.Models
{
    public partial class ContactInfo
    {
        public ContactInfo()
        {
            Email = new HashSet<Email>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ContactNumber { get; set; }

        public virtual ICollection<Email> Email { get; set; }
    }
}
