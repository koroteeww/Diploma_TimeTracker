using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebExplorer.Models.SiteUsers
{
    /// <summary>
    /// описание роли пользователя на сайте
    /// </summary>
    public class SiteUserRoleModel:ErrorsModel
    {
        /// <summary>
        /// Публичный ID
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }
        /// <summary>
        /// имя 
        /// </summary>
        [DataMember(Name = "type")]
        public int RoleType { get; set; }
        /// <summary>
        /// описание
        /// </summary>
        [DataMember(Name = "descr")]
        public string RoleDescription { get; set; }
    }
}