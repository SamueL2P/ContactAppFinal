using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppFinal.Models;
using FluentNHibernate.Mapping;

namespace ContactAppFinal.Mappings
{
    public class ContactMap:ClassMap<Contact>
    {
        public ContactMap() {
            Table("Contacts");
            Id(c => c.ContactId).GeneratedBy.GuidComb();
            Map(c => c.FName);
            Map(c => c.LName);
            Map(c => c.IsActive);
            HasMany(u => u.ContactDetail).Inverse().Cascade.All();
            References(o => o.User).Column("UserId").Cascade.None().Nullable();
        }
    }
}