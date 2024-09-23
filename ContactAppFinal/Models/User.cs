using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactAppFinal.Models
{
    public class User
    {
        public virtual Guid Id { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }

        public virtual string FName { get; set; }

        public virtual string LName { get; set; }

        public virtual bool IsAdmin { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual IList<Contact> Contact { get; set; } = new List<Contact>();

        public virtual Role Role { get; set; } = new Role();


    }
}