using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WebExplorer.Models.ClientsDepartments;

namespace WebExplorer.Models.SiteUsers
{
    public class SiteUsersListModel
    {
        /// <summary>
        /// Список user
        /// </summary>
        [DataMember(Name = "users")]
        public IEnumerable<SiteUserModel> Users { get; set; }
    }
}