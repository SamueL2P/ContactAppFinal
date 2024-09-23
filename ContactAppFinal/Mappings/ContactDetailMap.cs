using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppFinal.Models;
using FluentNHibernate.Mapping;

namespace ContactAppFinal.Mappings
{
    public class ContactDetailMap:ClassMap<ContactDetail>
    {
        public ContactDetailMap() {
            Table("ContactDetails");
            Id(d => d.ContactDetailId).GeneratedBy.GuidComb();
            Map(d => d.Type);
            Map(d => d.Value);
            References(o => o.Contact).Column("ContactId").Cascade.None().Nullable();
        }
    }
}