using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// Модель редактирования комментария
    /// </summary>
    public class EditCommentModel : ErrorsModel
    {
        /// <summary>
        /// Идентификатор комментария
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// Тело комментария
        /// </summary>
        [Required(ErrorMessage = "Забыли написать текст?")]
        [MinLength(3, ErrorMessage = "Слишком короткий текст комментария")]
        [DataMember(Name = "body")]
        public string Body { get; set; }
    }
}