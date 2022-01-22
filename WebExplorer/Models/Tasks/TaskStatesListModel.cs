using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// список возможных состояний задачи
    /// </summary>
    public class TaskStatesListModel
    {
        /// <summary>
        /// Список 
        /// </summary>
        [DataMember(Name = "types")]
        public IEnumerable<Tasks.TaskStateModel> TaskStates { get; set; }
    }
}

