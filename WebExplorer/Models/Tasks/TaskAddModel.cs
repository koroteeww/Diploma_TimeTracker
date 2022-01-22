using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebExplorer.Models.Tasks
{
    /// <summary>
    /// модель добавления задачи
    /// </summary>
    public class TaskAddModel : ErrorsModel
    {
        /// <summary>
        /// author id
        /// </summary>
        [DataMember(Name = "author")]
        public int AuthorId { get; set; }
        /// <summary>
        /// имя задачи
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
        /// <summary>
        /// описание задачи
        /// </summary>
        [DataMember(Name = "descr")]
        public string Description { get; set; }
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
        /// <summary>
        /// comments
        /// </summary>
        [DataMember(Name = "comments")]
        public string Comments { get; set; }
        /// <summary>
        /// DateEnd plan
        /// </summary>
        [DataMember(Name = "plannedDateEnd")]
        public DateTime PlannedDateEnd { get; set; }
        /// <summary>
        /// timecost plan
        /// </summary>
        [DataMember(Name = "PlannedTimeCost")]
        public decimal PlannedTimeCost { get; set; }
        /// <summary>
        /// state id
        /// </summary>
        [DataMember(Name = "stateid")]
        public int StateId { get; set; }
        /// <summary>
        /// state
        /// </summary>
        [DataMember(Name = "state")]
        public TaskStateModel State { get; set; }
        /// <summary>
        /// type id
        /// </summary>
        [DataMember(Name = "typeid")]
        public int TypeId { get; set; }
        /// <summary>
        /// type
        /// </summary>
        [DataMember(Name = "type")]
        public TaskTypeModel Type { get; set; }
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
        /// performers
        /// </summary>
        [DataMember(Name = "performers")]
        public List<SiteUserModel> TaskPerformers { get; set; }
        /// <summary>
        /// clients
        /// </summary>
        [DataMember(Name = "clients")]
        public List<ClientsDepartments.ClientModel> TaskClients { get; set; }
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
        [DataMember(Name = "PlannedDateEndForDisplay")]
        public string PlannedDateEndForDisplay
        {
            get
            {
                return this.PlannedDateEnd.ToString("dd.MM.yyyy");
            }
            set { PlannedDateEnd = DateTime.Parse(value); }
        }
        /// <summary>
        /// performers ids
        /// </summary>
        [DataMember(Name = "WorkersIds")]
        public Dictionary<string,int> WorkersIdsNames { get; set; }
        [DataMember(Name = "SelectedCheckBoxWorkersListItems")]
        public int[] SelectedCheckBoxWorkersListItems { get; set; }
        /// <summary>
        /// clients ids
        /// </summary>
        [DataMember(Name = "ClientsIdsNames")]
        public Dictionary<string, int> ClientsIdsNames { get; set; }
        [DataMember(Name = "SelectedCheckBoxClientsListItems")]
        public int[] SelectedCheckBoxClientsListItems { get; set; }
    }
}