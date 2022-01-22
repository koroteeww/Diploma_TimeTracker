using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebExplorer.Models.Tasks
{
    /// <summary>
    /// модель задачи для Листа
    /// </summary>
    public class TaskModelForList : ErrorsModel
    {
        /// <summary>
        /// time costs
        /// </summary>
        [DataMember(Name = "costs")]
        public decimal costs { get; set; }
        /// <summary>
        /// Публичный ID
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// имя 
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
        /// <summary>
        /// pefrormers 
        /// </summary>
        [DataMember(Name = "pefrormers")]
        public string Pefrormers { get; set; }
        /// <summary>
        /// Clients 
        /// </summary>
        [DataMember(Name = "Clients")]
        public string Clients { get; set; }
        /// <summary>
        /// описание
        /// </summary>
        [DataMember(Name = "descr")]
        public string Description { get; set; }
        /// <summary>
        /// DateBegin
        /// </summary>
        [DataMember(Name = "DateBegin")]
        public DateTime DateBegin { get; set; }
        /// <summary>
        /// DateBegin s
        /// </summary>
        [DataMember(Name = "DateBegins")]
        public String DateBeginS { get; set; }
        /// <summary>
        /// DateEnd
        /// </summary>
        [DataMember(Name = "DateEnd")]
        public DateTime DateEnd { get; set; }
        /// <summary>
        /// Dateend s
        /// </summary>
        [DataMember(Name = "DateEnds")]
        public String DateEndS { get; set; }
        /// <summary>
        /// comments
        /// </summary>
        [DataMember(Name = "comments")]
        public string Comments { get; set; }
        /// <summary>
        /// author name
        /// </summary>
        [DataMember(Name = "authorname")]
        public string AuthorName { get; set; }
        /// <summary>
        /// state
        /// </summary>
        [DataMember(Name = "state")]
        public string State { get; set; }
        /// <summary>
        /// type
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}