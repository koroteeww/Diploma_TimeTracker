using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebExplorer.Models.ClientsDepartments
{
    [DataContract]
    public class DepartmentAddModel : ErrorsModel
    {
            /// <summary>
            /// Публичный ID
            /// </summary>
            [DataMember(Name = "id")]
            public int? Id { get; set; }

            /// <summary>
            /// Название отдела
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; set; }

            /// <summary>
            /// ид родительского отдела, может быть не задан
            /// </summary>
            [DataMember(Name = "parentId")]
            public int? ParentId { get; set; }
            /// <summary>
            /// имя родительского отдела, может быть не задан
            /// </summary>
            [DataMember(Name = "parentName")]
            public string ParentName { get; set; }
            /// <summary>
            /// Список существующих отделов которые можно поставить родительскими
            /// </summary>
            [DataMember(Name = "departments")]
            public List<DepartmentModel> ExistingDepartments { get; set; }
            
        
    }
    }
