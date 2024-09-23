using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppFinal.Models;
using FluentNHibernate.Mapping;

namespace ContactAppFinal.Mappings
{
    public class UserMap:ClassMap<User>
    {
        public UserMap() {
            Table("Users");
            Id(u => u.Id).GeneratedBy.GuidComb();
            Map(u => u.UserName);
            Map(u => u.Password);
            Map(u => u.FName);
            Map(u => u.LName);
            Map(u => u.IsAdmin);
            Map(u => u.IsActive);
            HasOne(u => u.Role).PropertyRef(u => u.User).Cascade.All().Constrained();
            HasMany(u => u.Contact).Inverse().Cascade.All();
        }
    }
}