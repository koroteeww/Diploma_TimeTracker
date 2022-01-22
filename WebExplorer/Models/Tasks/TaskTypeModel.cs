using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebExplorer.Models.Tasks
{
    public class TaskTypeModel:ErrorsModel
    {
        /// <summary>
        /// Публичный ID
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }
        /// <summary>
        /// имя 
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
        /// <summary>
        /// описание
        /// </summary>
        [DataMember(Name = "d")]
        public string Description { get; set; }
    }
}