using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// дерево отделов
    /// </summary>
    public class DepartmentTreeModel
    {
        /// <summary>
        /// Список отделов
        /// </summary>
        [DataMember(Name = "departments")]
        public IEnumerable<DepartmentModel> Departments { get; set; }

        
    }
}