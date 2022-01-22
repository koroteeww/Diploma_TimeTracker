using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebExplorer.Models
{
    /// <summary>
    /// описание отдела
    /// </summary>
    [DataContract]
    public class DepartmentModel
    {
        
            /// <summary>
            /// Публичный ID
            /// </summary>
            [DataMember(Name = "id")]
            public int Id { get; set; }

            /// <summary>
            /// Название отдела
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; set; }

            /// <summary>
            /// ид родительского отдела, может быть не задан
            /// </summary>
            [DataMember(Name = "parentId")]
            public Nullable<int> ParentId { get; set; }
            /// <summary>
            /// имя родительского отдела, может быть не задан
            /// </summary>
            [DataMember(Name = "parentName")]
            public string ParentName { get; set; }
            
        }
    }
