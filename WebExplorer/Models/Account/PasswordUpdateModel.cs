using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// модель восстановления пароля
    /// </summary>
    [DataContract]
    public class PasswordUpdateModel : ErrorsModel
    {
        /// <summary>
        /// Хеш
        /// </summary>        
        [DataMember(Name = "hash")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Секретный хэш неверен. Попробуйте еще раз.")]
        public string Hash { get; set; }
        /// <summary>
        /// Новый пароль, введенный юзером.
        /// </summary>
        [DataMember(Name = "newpassword")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Забыли ввести пароль")]        
        [MinLength(8, ErrorMessage = "Минимальная длина пароля - 8 символов")]
        [RegularExpression(".*(([a-z].*[A-Z])|([A-Z].*[a-z])).*", ErrorMessage = "Пароль должен содержать буквы в нижнем и верхнем регистре")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Признак, того что хэш валиден
        /// </summary>
        public bool HashValid { get; set; }
    }
}