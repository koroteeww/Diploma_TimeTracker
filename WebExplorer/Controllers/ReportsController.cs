using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebExplorer.Auth;
using WebExplorer.Core;
using WebExplorer.Entity;
using WebExplorer.Models;
using WebExplorer.Models.ClientsDepartments;
using WebExplorer.Models.SiteUsers;
using WebExplorer.Models.Tasks;
using System.Data.Entity;

namespace WebExplorer.Controllers
{
    public class ReportsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            //отчеты:
            //Админу показываются отчеты такие же как пользователю
            //Начальнику показываются отчеты с возможностью выбора любого пользователя или нескольких(??)
            //плюс оценка запланированных сроков и затра
            //плюс учесть то что на одну задачу может быть куча исполнителей
            //
            //Пользователю показывается отчет о затратах времени по задачам:
            //задача, дата начала-окончания,затраченное время ЭТОГО ПОЛЬЗОВАТЕЛЯ, комменты и т.д.
            var reportmodel = new ReportIndexModel();
            var curUser = WeMembership.GetCurrentUser();
            reportmodel.DateBegin = DateTime.Now.AddDays(-10);
            reportmodel.DateEnd = DateTime.Now;
            using (var db = new WeContext())
            {
                //clients
                List<ClientModel> list=new List<ClientModel>();
                var ll = db.ClientSet.OrderBy(cl => cl.LastName).ToList();
                foreach (var client in ll)
                {
                    list.Add(client.ToClientModel());
                }
                reportmodel.Clients=new List<ClientModel>();
                
                reportmodel.Clients = list;
                reportmodel.Clients.Add(new ClientModel { Id = 0, FirstName = "(не важно)", LastName = "", MiddleName = "" });
                //performers
                reportmodel.Performers =new List<SiteUserModel>();
                
                if (curUser.IsAdmin)
                {
                    reportmodel.Performers =
                        db.SiteUserSet.OrderBy(user => user.Login).ToList().Select(user => user.ToSiteUserModel()).ToList();
                }
                else if (curUser.CompanyWorker.IsManager)
                {
                    reportmodel.Performers =
                        Common.GetAvailableClients(curUser).Select(c=>c.ToSiteUserModel()).ToList();
                }
                else
                {
                    reportmodel.Performers.Add(curUser.ToSiteUserModel());
                }
                reportmodel.Performers.Add(new SiteUserModel { Id = 0, WorkerName = "(не важно)" });
                //task types
                reportmodel.TaskTypes=new List<TaskTypeModel>();
                
                reportmodel.TaskTypes = db.TaskTypesSet.Select(tt => new TaskTypeModel
                    {
                        Id = tt.Id,
                        Name = tt.TypeName,
                        Description = tt.TypeDescription
                    }).ToList();
                reportmodel.TaskTypes.Add(new TaskTypeModel
                {
                    Id = 0,
                    Name = "(не важно)",
                    Description = ""
                });
                //task states
                reportmodel.TaskStates=new List<TaskStateModel>();
                
                reportmodel.TaskStates = db.TastStatesSet.Select(ts => new TaskStateModel
                {
                    Id = ts.Id,
                    Name = ts.StateName,
                    Description = ts.StateDescription
                }).ToList();
                reportmodel.TaskStates.Add(new TaskStateModel
                {
                    Id = 0,
                    Name = "(не важно)",
                    Description = ""
                });
            }

            return View(reportmodel);
        }
        [HttpPost]
        public ActionResult Index(ReportIndexModel indexModel)
        {
            var model = new ReportModel();
            model.TaskReportModelList = new List<TaskModelForList>();
            using (var db = new WeContext())
            {
                //var dateBegin = indexModel.DateBegin ?? DateTime.Now.AddDays(-10);
                //var preformer = db.SiteUserSet.FirstOrDefault(uset => uset.Id == indexModel.PerformerId);

                var taskSetList = db.TaskSet
                    .Where(task => (task.DateBegin >= indexModel.DateBegin) && (task.DateEnd <= indexModel.DateEnd))
                    .Where(task => (task.TaskType.Id == indexModel.TypeId) || (indexModel.TypeId == 0))
                    .Where(task => (task.TastState.Id == indexModel.StateId) || (indexModel.StateId == 0))
                    .Where(task => (task.TaskPerformers.Any(p => p.Id == indexModel.PerformerId) || (indexModel.PerformerId == 0)))
                    .Where(task => (task.TaskClients.Any(cl => cl.Id == indexModel.ClientId) || indexModel.ClientId == 0))
                    .OrderByDescending(n => n.DateBegin).ToList();


                var tasksModel = taskSetList.Select(taskset => new TaskModelForList
                {
                    Id = taskset.Id,
                    Name = taskset.Name,
                    Description = taskset.Description ?? String.Empty,
                    AuthorName = taskset.TaskAuthor.CompanyWorker.ToClientModel().Name,
                    DateBegin = taskset.DateBegin,
                    DateBeginS = taskset.DateBegin.ToLongDateString(),
                    DateEnd = taskset.DateEnd,
                    DateEndS = taskset.DateEnd.ToLongDateString(),
                    Comments = taskset.Comments ?? string.Empty,
                    State = taskset.TastState.StateName,
                    Type = taskset.TaskType.TypeName,
                    costs = taskset.TaskTimeCosts.Sum(d => d.SpentHours)

                }).ToList();
                //tasksModel = addClientsPerformers(tasksModel);
                //список 
                tasksModel = Common.addClientsPerformers(tasksModel);
                model.TaskReportModelList = tasksModel;
                model.CostsSum = tasksModel.Sum(t => t.costs);
                return View("Report", model);
            }
            
            
            
        }
        
    }
}