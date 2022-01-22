using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// Информация о редактировании профиля
    /// </summary>
    [DataContract]
    public class AccountModel : ErrorsModel
    {
        /// <summary>
        /// Логин
        /// </summary>
        [DataMember(Name = "login")]
        public string Login { get; set; }

        /// <summary>
        /// Новый пароль
        /// </summary>
        [DataMember(Name = "password")]
        [MinLength(8,ErrorMessage = "Минимальная длина пароля - 8 символов")]
        [RegularExpression(".*(([a-z].*[A-Z])|([A-Z].*[a-z])).*", ErrorMessage = "Пароль должен содержать буквы в нижнем и верхнем регистре")]
        public string Password { get; set; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        [DataMember(Name = "email")]
        [EmailAddress(ErrorMessage = "← это не похоже на email")]
        public string Email { get; set; }
    }
}