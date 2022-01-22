using System;
using System.Runtime.Serialization;
using WebExplorer.Entity;
using WebExplorer.Models.ClientsDepartments;

namespace WebExplorer.Models
{
    /// <summary>
    /// модель пользователя
    /// </summary>
    public class SiteUserModel
    {
        
        /// <summary>
        /// Публичный ID
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }
        /// <summary>
        /// Creation date
        /// </summary>
        [DataMember(Name = "Creation")]
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// e-mail
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }
        /// <summary>
        /// Login
        /// </summary>
        [DataMember(Name = "login")]
        public string Login { get; set; }
        /// <summary>
        /// approve
        /// </summary>
        [DataMember(Name = "approved")]
        public string IsApproved { get; set; }
        /// <summary>
        /// locked
        /// </summary>
        [DataMember(Name = "locked")]
        public string IsLocked { get; set; }
        /// <summary>
        /// admin
        /// </summary>
        [DataMember(Name = "admin")]
        public string IsAdmin { get; set; }
        /// <summary>
        /// comment
        /// </summary>
        [DataMember(Name = "comment")]
        public string Comment { get; set; }
        /// <summary>
        /// role
        /// </summary>
        [DataMember(Name = "role")]
        public string SiteUserRole { get; set; }
        /// <summary>
        /// worker
        /// </summary>
        [DataMember(Name = "worker")]
        public string WorkerName { get; set; }
        /// <summary>
        /// workerid
        /// </summary>
        [DataMember(Name = "workerid")]
        public int WorkerId { get; set; }
        /// <summary>
        /// role id
        /// </summary>
        [DataMember(Name = "siteroleid")]
        public int SiteUserRoleId { get; set; }
    }
}