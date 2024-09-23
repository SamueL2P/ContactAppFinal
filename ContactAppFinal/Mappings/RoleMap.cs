﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppFinal.Models;
using FluentNHibernate.Mapping;

namespace ContactAppFinal.Mappings
{
    public class RoleMap:ClassMap<Role>
    {
        public RoleMap() {
            Table("Roles");
            Id(r => r.Id).GeneratedBy.GuidComb();
            Map(r => r.RoleName);
            References(r => r.User).Column("UserId").Cascade.None().Unique();   
        }
    }
}