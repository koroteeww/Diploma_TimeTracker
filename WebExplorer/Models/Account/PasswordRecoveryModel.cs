using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// модель восстановления пароля
    /// </summary>
    [DataContract]
    public class PasswordRecoveryModel : ErrorsModel
    {
        /// <summary>
        /// Е-мейл для восстановления пароля
        /// </summary>
        [DataMember(Name = "email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Забыли ввести почту")]        
        [EmailAddress(ErrorMessage = "← это не похоже на email")]
        public string Email { get; set; }

        /// <summary>
        /// Признак того, что письмо уже отослано
        /// </summary>
        public bool LetterSent { get; set; }
    }
}