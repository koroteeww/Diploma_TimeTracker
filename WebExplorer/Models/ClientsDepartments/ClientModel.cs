using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebExplorer.Models.ClientsDepartments
{
    /// <summary>
    /// описание клиента
    /// </summary>
    public class ClientModel
    {
        /// <summary>
        /// Публичный ID
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// ФИО сотрудника
        /// </summary>
        [DataMember(Name = "name")]
        public string Name
        {
            get { return LastName + " " + FirstName + " " + MiddleName; }
            
        }
        /// <summary>
        /// ФИО сотрудника и отдел и должность (для списков)
        /// </summary>
        [DataMember(Name = "fullname")]
        public string NameDptPos
        {
            get { return Name+String.Format(" ({0}, {1})",Position,DepartmentName); }

        }
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
        /// является ли сотрудник начальником string
        /// </summary>
        [DataMember(Name = "managerstring")]
        public string IsManagerString { get; set; }
        /// <summary>
        /// комментарии
        /// </summary>
        [DataMember(Name = "comment")]
        public string Comment { get; set; }
        /// <summary>
        /// id отдела
        /// </summary>
        [DataMember(Name = "dpt")]
        public int DepartmentId { get; set; }
        /// <summary>
        /// name отдела
        /// </summary>
        [DataMember(Name = "dptname")]
        public string DepartmentName { get; set; }
    }
}