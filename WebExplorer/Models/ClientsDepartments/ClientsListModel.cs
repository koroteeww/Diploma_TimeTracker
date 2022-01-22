using System.Collections.Generic;
using System.Runtime.Serialization;
using WebExplorer.Models.ClientsDepartments;

namespace WebExplorer.Models
{
    /// <summary>
    /// список "клиентов"
    /// в нашем случае это только работники предприятия
    /// </summary>
    public class ClientsListModel
    {
        /// <summary>
        /// Список сотрудников
        /// </summary>
        [DataMember(Name = "clients")]
        public IEnumerable<ClientModel> Clients { get; set; }
    }
}