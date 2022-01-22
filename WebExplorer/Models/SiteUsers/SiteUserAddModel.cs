using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebExplorer.Models.SiteUsers
{
    /// <summary>
    /// модель создания пользователя
    /// </summary>
    public class SiteUserAddModel:ErrorsModel
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
        /// password
        /// </summary>
        [DataMember(Name = "pass")]
        [MinLength(6, ErrorMessage = "Минимальная длина пароля - 6 символов")]
        [RegularExpression(".*(([a-z].*[A-Z])|([A-Z].*[a-z])).*", ErrorMessage = "Пароль должен содержать буквы в нижнем и верхнем регистре")]
        public string Pass { get; set; }
        /// <summary>
        /// salt
        /// </summary>
        [DataMember(Name = "hash")]
        public string Hash { get; set; }
        /// <summary>
        /// e-mail
        /// </summary>
        [DataMember(Name = "email")]
        [EmailAddress(ErrorMessage = "← это не похоже на email")]
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
        public bool IsApproved { get; set; }
        /// <summary>
        /// locked
        /// </summary>
        [DataMember(Name = "locked")]
        public bool IsLocked { get; set; }
        /// <summary>
        /// admin
        /// </summary>
        [DataMember(Name = "admin")]
        public bool IsAdmin { get; set; }
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
        public int? WorkerId { get; set; }
        /// <summary>
        /// role id
        /// </summary>
        [DataMember(Name = "siteroleid")]
        public int SiteUserRoleId { get; set; }
        /// <summary>
        /// доступные роли
        /// </summary>
        [DataMember(Name = "roles")]
        public List<SiteUserRoleModel> Roles { get; set; }
        /// <summary>
        /// доступные работники
        /// </summary>
        [DataMember(Name = "workers")]
        public List<ClientsDepartments.ClientModel> Workers { get; set; }
    }

    
}