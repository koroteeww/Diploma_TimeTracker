using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebExplorer.Entity;

namespace WebExplorer.Models.SiteUsers
{
    public static class SiteUserRoleModelExtensions
    {
        public static SiteUserRoleModel ToSiteUserRoleModel(this SiteUserRole role)
        {
            return new SiteUserRoleModel
            {
                Id = role.Id,
                RoleDescription = role.RoleDescription,
                RoleType = role.RoleType
            };
        }
    }
}