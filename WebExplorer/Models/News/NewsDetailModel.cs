using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// Детальная информация о новости
    /// </summary>
    [DataContract]
    public class NewsDetailModel : NewsModel
    {
        /// <summary>
        /// Комментарии к новости
        /// </summary>
        [DataMember(Name = "comments")]
        public IEnumerable<NewsCommentModel> Comments { get; set; } 
    }
}