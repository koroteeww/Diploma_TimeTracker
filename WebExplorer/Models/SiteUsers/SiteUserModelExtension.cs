using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebExplorer.Entity;

namespace WebExplorer.Models.SiteUsers
{
    public static class SiteUserModelExtension
    {
        public static string GetFIO(this SiteUser user)
        {
            return user.CompanyWorker.GetFIO();
        }
    }
}