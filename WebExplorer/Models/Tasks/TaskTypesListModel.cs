using System.Collections.Generic;
using System.Runtime.Serialization;
using WebExplorer.Models.ClientsDepartments;

namespace WebExplorer.Models
{
    /// <summary>
    /// список типов задач
    /// </summary>
    public class TaskTypesListModel
    {
        /// <summary>
        /// Список 
        /// </summary>
        [DataMember(Name = "types")]
        public IEnumerable<Tasks.TaskTypeModel> TaskTypes { get; set; }
    }
}