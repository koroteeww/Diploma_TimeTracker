using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WebExplorer.Models.Tasks;

namespace WebExplorer.Models
{
    /// <summary>
    /// отчеты:
    ///Админу показываются отчеты такие же как пользователю
    ///Начальнику показываются отчеты с возможностью выбора любого пользователя или нескольких(??)
    ///плюс оценка запланированных сроков и затра
    ///плюс учесть то что на одну задачу может быть куча исполнителей
    ///
    ///Пользователю показывается отчет о затратах времени по задачам:
    ///задача, дата начала-окончания,затраченное время ЭТОГО ПОЛЬЗОВАТЕЛЯ, комменты и т.д.
    /// </summary>
    public class ReportIndexModel:ErrorsModel
    {
        /// <summary>
        /// clients
        /// </summary>
        [DataMember(Name = "clients")]
        public List<ClientsDepartments.ClientModel> Clients { get; set; }
        [DataMember(Name = "DateBeginForDisplay")]
        public string DateBeginForDisplay
        {
            get
            {
                return this.DateBegin.ToString("dd.MM.yyyy");
            }
            set { DateBegin = DateTime.Parse(value); }
        }
        [DataMember(Name = "DateEndForDisplay")]
        public string DateEndForDisplay
        {
            get
            {
                return this.DateEnd.ToString("dd.MM.yyyy");
            }
            set { DateEnd = DateTime.Parse(value); }

        }
        /// <summary>
        /// performers
        /// </summary>
        [DataMember(Name = "performers")]
        public List<SiteUserModel> Performers { get; set; }
        /// <summary>
        /// available states
        /// </summary>
        [DataMember(Name = "states")]
        public List<TaskStateModel> TaskStates { get; set; }
        /// <summary>
        /// available types
        /// </summary>
        [DataMember(Name = "types")]
        public List<TaskTypeModel> TaskTypes { get; set; }
        /// <summary>
        /// type id
        /// </summary>
        [DataMember(Name = "typeid")]
        public int TypeId { get; set; }
        /// <summary>
        /// state id
        /// </summary>
        [DataMember(Name = "stateid")]
        public int StateId { get; set; }
        /// <summary>
        /// ClientId id
        /// </summary>
        [DataMember(Name = "ClientId")]
        public int ClientId { get; set; }
        /// <summary>
        /// PerformerId id
        /// </summary>
        [DataMember(Name = "PerformerId")]
        public int PerformerId { get; set; }
        /// <summary>
        /// DateBegin
        /// </summary>
        [DataMember(Name = "DateBegin")]
        public DateTime DateBegin { get; set; }
        /// <summary>
        /// DateEnd
        /// </summary>
        [DataMember(Name = "DateEnd")]
        public DateTime DateEnd { get; set; }
    }
}