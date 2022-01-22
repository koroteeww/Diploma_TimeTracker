using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// Класс модели сохранения новости
    /// </summary>
    [DataContract]
    public class NewsEditModel : ErrorsModel
    {
        /// <summary>
        /// Идентификатор новости
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// Заголовок новости
        /// </summary>
        [Required(ErrorMessage = "Забыли ввести заголовок?")]
        [MinLength(3, ErrorMessage = "Слишком короткий загловок")]
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Тело новости
        /// </summary>
        [Required(ErrorMessage = "Забыли написать текст?")]
        [MinLength(3, ErrorMessage = "Слишком короткий текст новости")]
        [DataMember(Name = "title")]
        public string Body { get; set; }
    }
}