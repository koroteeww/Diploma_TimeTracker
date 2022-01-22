using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WebExplorer.Models.Tasks;

namespace WebExplorer.Models
{
    /// <summary>
    /// report
    /// </summary>
    public class ReportModel : ErrorsModel
    {
        public List<TaskModelForList> TaskReportModelList { get; set; }
        public decimal CostsSum { get; set; }
    }
    
}