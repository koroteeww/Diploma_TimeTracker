using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// Админу показываются все задачи с возможностью редактить или удалять
    ///Начальнику показываются задачи пользователей из его отдела (и из подчиненных отдельной кнопкой)
    ///Пользователю показываются только его задачи
    /// </summary>
    public class TaskListModel
    {
        /// <summary>
        /// Список 
        /// </summary>
        [DataMember(Name = "tasks")]
        public IEnumerable<Tasks.TaskModelForList> Tasks { get; set; }
    }
}