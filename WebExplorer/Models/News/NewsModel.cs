using System.Runtime.Serialization;
using WebExplorer.Auth;

namespace WebExplorer.Models
{
    /// <summary>
    /// Описание новости
    /// </summary>
    [DataContract]
    public class NewsModel
    {
        /// <summary>
        /// Публичный ID
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Текст новости
        /// </summary>
        [DataMember(Name = "body")]
        public string Body { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        [DataMember(Name = "date")]
        public string Date { get; set; }

        /// <summary>
        /// Автор новости
        /// </summary>
        [DataMember(Name = "author")]
        public string Author { get; set; }

        /// <summary>
        /// Идентификатор автора новости
        /// </summary>
        [DataMember(Name = "authorId")]
        public int AuthorId { get; set; }

        /// <summary>
        /// Можно ли редактировать
        /// </summary>
        [DataMember(Name = "canEdit")]
        public bool CanEdit { get; set; }

        /// <summary>
        /// Количество коментариев
        /// </summary>
        [DataMember(Name = "commentsCount")]
        public int CommentsCount { get; set; }
    }
}