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
    
    public partial class NewsComments
    {
        public int Id { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Body { get; set; }
    
        public virtual News News { get; set; }
        public virtual SiteUser Author { get; set; }
    }
}