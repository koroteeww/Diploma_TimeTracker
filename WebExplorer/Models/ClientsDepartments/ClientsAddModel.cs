using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebExplorer.Models.ClientsDepartments
{
    /// <summary>
    /// добавление клиента
    /// </summary>
    public class ClientsAddModel:ErrorsModel
    {
        /// <summary>
        /// Публичный ID
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// имя сотрудника
        /// </summary>
        [DataMember(Name = "firstname")]
        public string FirstName { get; set; }
        /// <summary>
        /// фамилия сотрудника
        /// </summary>
        [DataMember(Name = "lastname")]
        public string LastName { get; set; }
        /// <summary>
        /// отчество сотрудника
        /// </summary>
        [DataMember(Name = "middlename")]
        public string MiddleName { get; set; }

        /// <summary>
        /// кабинет сотрудника
        /// </summary>
        [DataMember(Name = "location")]
        public string Location { get; set; }
        /// <summary>
        /// должность
        /// </summary>
        [DataMember(Name = "position")]
        public string Position { get; set; }
        /// <summary>
        /// организация
        /// </summary>
        [DataMember(Name = "company")]
        public string Company { get; set; }
        /// <summary>
        /// телефонный номер
        /// </summary>
        [DataMember(Name = "phone")]
        public string Phone { get; set; }
        /// <summary>
        /// является ли сотрудник начальником
        /// </summary>
        [DataMember(Name = "manager")]
        public bool IsManager { get; set; }
        
        /// <summary>
        /// комментарии
        /// </summary>
        [DataMember(Name = "comment")]
        public string Comment { get; set; }
        
        


        /// <summary>
        /// ид отдела, может быть не задан
        /// </summary>
        [DataMember(Name = "dptId")]
        public int? DepartmentId { get; set; }
        /// <summary>
        /// Список существующих отделов которые можно поставить 
        /// </summary>
        [DataMember(Name = "departments")]
        public List<DepartmentModel> ExistingDepartments { get; set; }
    }
}