using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestAPI2.Models
{
    public partial class Email
    {
        public int Id { get; set; }
        public int? ContactInfoId { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public virtual ContactInfo ContactInfo { get; set; }
    }
}
