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
    
    public partial class TaskTypes
    {
        public TaskTypes()
        {
            this.Task = new HashSet<Task>();
        }
    
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string TypeDescription { get; set; }
    
        public virtual ICollection<Task> Task { get; set; }
    }
}