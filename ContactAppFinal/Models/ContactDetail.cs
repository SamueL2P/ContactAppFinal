using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ContactAppFinal.Models
{
    public class ContactDetail
    {
        public virtual Guid ContactDetailId { get; set; }
            
        [DisplayName("Type Number/Email ")]
        public virtual string Type { get; set; }

        public virtual string Value { get; set; }

        public virtual Contact Contact { get; set; }
    }
}