using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebExplorer.Models
{
    /// <summary>
    /// Модель списка новостей
    /// </summary>
    [DataContract]
    public class NewsListModel
    {
        /// <summary>
        /// Список новостей
        /// </summary>
        [DataMember(Name = "news")]
        public IEnumerable<NewsModel> News { get; set; }

        /// <summary>
        /// Общее количество новостей
        /// </summary>
        [DataMember(Name = "totalNewsCount")]
        public int TotalNewsCount { get; set; }

        /// <summary>
        /// Общее количество страниц
        /// </summary>
        [DataMember(Name = "totalPagesCount")]
        public int TotalPagesCount { get; set; }

        /// <summary>
        /// Текущая страница
        /// </summary>
        [DataMember(Name = "currentPage")]
        public int CurrentPage { get; set; }
    }
}