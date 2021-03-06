//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebExplorer.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Client
    {
        public Client()
        {
            this.IsManager = false;
            this.TasksForClient = new HashSet<Task>();
        }
    
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Location { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsManager { get; set; }
        public string Comment { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual SiteUser SiteUser { get; set; }
        public virtual ICollection<Task> TasksForClient { get; set; }

        public string GetFIO()
        {
            return FirstName + LastName + MiddleName;
        }
    }
}
