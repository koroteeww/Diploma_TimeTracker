using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// Модель авторизации
    /// </summary>
    [DataContract]
    public class LogOnModel : ErrorsModel
    {
        /// <summary>
        /// Логин
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Забыли ввести логин")]
        [DataMember(Name = "login")]
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Нельзя войти без пароля")]
        [DataMember(Name = "password")]
        public string Password { get; set; }

        /// <summary>
        /// Постоянная авторизация
        /// </summary>
        [DataMember(Name = "remember")]
        public bool Remember { get; set; }

        /// <summary>
        /// Адрес возврата
        /// </summary>
        [DataMember(Name = "returnUrl")]
        public string ReturnUrl { get; set; } 
    }
}