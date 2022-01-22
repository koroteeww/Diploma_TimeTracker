using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// Комментарий к новости
    /// </summary>
    [DataContract]
    public class NewsCommentModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// Автор комментария
        /// </summary>
        [DataMember(Name = "author")]
        public string Author { get; set; }
        /// <summary>
        /// wildboar
        /// Идентификатор автора комментария
        /// </summary>
        [DataMember(Name = "authorId")]
        public int AuthorId { get; set; }
        /// <summary>
        /// wildboar
        /// Идентификатор новости,которой принадлежит комментарий
        /// </summary>
        [DataMember(Name = "authorId")]
        public int NewsId { get; set; }

        /// <summary>
        /// Дата комментария
        /// </summary>
        [DataMember(Name = "date")]
        public string Date { get; set; }
        /// <summary>
        /// Текст комментария
        /// </summary>
        [DataMember(Name = "body")]
        public string Body { get; set; }

        /// <summary>
        /// wildboar
        /// Можно ли редактировать
        /// </summary>
        [DataMember(Name = "canEdit")]
        public bool CanEdit { get; set; }
    }
}