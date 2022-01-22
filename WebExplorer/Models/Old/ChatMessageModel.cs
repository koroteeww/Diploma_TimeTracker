using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// Информация о сообщении чата
    /// </summary>
    [DataContract]
    public class ChatMessageModel
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        [DataMember(Name = "body")]
        public string Body { get; set; }

        /// <summary>
        /// Автор сообщения
        /// </summary>
        [DataMember(Name = "author")]
        public string Author { get; set; }

        /// <summary>
        /// Идентификатор автора сообщения
        /// </summary>
        [DataMember(Name = "authorId")]
        public int AuthorId { get; set; }

        /// <summary>
        /// Дата отправки сообщения
        /// </summary>
        [DataMember(Name = "date")]
        public string Date { get; set; }
    }
}