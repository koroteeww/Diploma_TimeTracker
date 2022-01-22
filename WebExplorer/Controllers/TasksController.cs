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
    
    public class TasksController : Controller
    {
        //
        // GET: /Tasks/
        private const string dateFormat = "dd-mm-yyyy";
        
        public ActionResult Index()
        {
            if (!WeMembership.IsAuthorized)
                return RedirectToAction("Index", "Home");

            //Админу показываются все задачи с возможностью редактить или удалять
            //Начальнику показываются задачи пользователей из его отдела (и из подчиненных отдельной кнопкой)
            //Пользователю показываются только его задачи (где он исполнитель)
            using (var db = new WeContext())
            {

                var curuser1 = WeMembership.GetCurrentUser();
                var curuser = db.SiteUserSet.FirstOrDefault(s => s.Id == curuser1.Id);

                if (curuser.IsAdmin)
                {
                    //список для Администратора сайта
                    var taskSetList = db.TaskSet.OrderByDescending(n => n.DateBegin).ToList();
                    
                    
                    var tasksModel= taskSetList.Select(taskset => new TaskModelForList
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
                            costs = taskset.TaskTimeCosts.Sum(d=>d.SpentHours)
                            
                        }).ToList();
                    tasksModel = addClientsPerformers(tasksModel);
                    //список 
                    var model = new TaskListModel
                        {

                            Tasks = tasksModel.OrderByDescending(d=>d.DateBegin)
                        };
                    return View(model);
                }
                else if (curuser.CompanyWorker.IsManager)
                {
                    //список для начальника
                    //сначала задачи где он исполнитель или автор
                    var taskSetList = db.TaskSet
                        .Where(task => task.TaskAuthor.Id == curuser.Id || task.TaskPerformers.Any(performer => performer.Id == curuser.Id))
                        .ToList();
                    //а еще задачи где автор или исполнитель - пользователи его отдела. или подчиненных
                    var curDpt = curuser.CompanyWorker.Department;
                    //подчиненные отделы (корневой и все "дочки")
                    var subDpr = Common.GetSubDepartments(curDpt);
                    var clientList =new List<Client>();
                    //идем и выбираем всех пользователей с этих отделов 
                    foreach (var dpt in subDpr)
                    {
                        var dpt1 = dpt;
                        clientList.AddRange(db.ClientSet.Where(c=>c.Department.Id==dpt1.Id).ToList());
                    }
                    var subList=new List<Task>();
                    //выбираем задачи (кроме себя)
                    foreach (var cl1 in clientList.Where(client => client.SiteUser != null && client.Id != curuser.CompanyWorker.Id))
                    {
                        Client cl2 = cl1;
                        var list = db.TaskSet.Where(
                            t => t.TaskAuthor.Id == cl2.SiteUser.Id ||
                                 t.TaskPerformers.Any(performer => performer.Id == cl2.SiteUser.Id)).ToList();
                        subList.AddRange(list);
                    }
                    //var taskSetListDpt = db.TaskSet.Where()

                    var tasksModel = new List<TaskModelForList>();
                    tasksModel.AddRange(taskSetList
                        .Select(taskset => new TaskModelForList
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
                    }).ToList());
                    tasksModel.AddRange(subList.Select(taskset => new TaskModelForList
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
                    }).ToList()); 
                    //список 
                    tasksModel = addClientsPerformers(tasksModel);
                    var comparer = new TaskModelForListComparer();
                    var model = new TaskListModel
                    {

                        Tasks = tasksModel.Distinct(comparer).OrderByDescending(t => t.DateBegin)
                    };
                    return View(model);
                }
                else
                {
                    //список для работника
                    var taskSetList = db.TaskSet
                        .OrderByDescending(n => n.DateBegin)
                        .Where(task => task.TaskAuthor.Id == curuser.Id || task.TaskPerformers.Any(user => user.Id == curuser.Id))
                        .ToList();
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
                    //список 
                    tasksModel = addClientsPerformers(tasksModel);
                    var comparer = new TaskModelForListComparer();
                    var model = new TaskListModel
                    {

                        Tasks = tasksModel.Distinct(comparer).OrderByDescending(t => t.DateBegin)
                    };
                    return View(model);
                }

            }
            
            
/*
            return View(new TaskListModel());
*/
        }

        private List<TaskModelForList> addClientsPerformers(List<TaskModelForList> tasksModel)
        {
            using (var dd=new WeContext())
            {
                foreach (var taskModelForList in tasksModel)
                {
                    var task = dd.TaskSet.FirstOrDefault(t => t.Id == taskModelForList.Id);
                    foreach (var t in task.TaskClients)
                    {
                        taskModelForList.Clients += t.GetFIO() + " ; ";
                    }
                    foreach (var t in task.TaskPerformers)
                    {
                        taskModelForList.Pefrormers += t.GetFIO() + " ; ";
                    }
                }
                
            }
            return tasksModel;
        }
        /// <summary>
        /// get add
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add()
        {
            var curuser1 = WeMembership.GetCurrentUser();
            
            using (var db = new WeContext())
            {
                var curuser = db.SiteUserSet.FirstOrDefault(s => s.Id == curuser1.Id);
                var performers = Common.GetAvailableClients(curuser);
                var model = new TaskAddModel
                    {
                        
                        TaskTypes    = db.TaskTypesSet.Select(tt=>new TaskTypeModel
                            {
                                Id = tt.Id,
                                Name = tt.TypeName,
                                Description = tt.TypeDescription
                            }).ToList(),

                    };
                model.StateId = TaskStatesFormatter.ToInt(TaskStates.Created);
                //model.TaskPerformers = performers;
                model.WorkersIdsNames=new Dictionary<string, int>();
                foreach (var siteUserModel in performers)
                {
                    model.WorkersIdsNames.Add(siteUserModel.GetFIO(), siteUserModel.Id);
                }
                model.ClientsIdsNames = new Dictionary<string, int>();
                foreach (var client in db.ClientSet.ToList())
                {
                    model.ClientsIdsNames.Add(client.GetFIO(),client.Id);
                }
                model.DateBegin = DateTime.Now;
                model.DateEnd = DateTime.Now.AddDays(1);
                model.PlannedDateEnd = DateTime.Now.AddDays(1);
                return View(model);
            }
        }
        /// <summary>
        /// post add
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(TaskAddModel model)
        {
            var curuser1 = WeMembership.GetCurrentUser();

            using (var db = new WeContext())
            {
                var author = db.SiteUserSet.FirstOrDefault(s => s.Id == curuser1.Id);
                //selected workers
                var workers = 
                    model.SelectedCheckBoxWorkersListItems.Select(
                    workerid => db.SiteUserSet.FirstOrDefault(w => w.Id == workerid)).ToList();
                //clients
                var clients =
                    model.SelectedCheckBoxClientsListItems.Select(
                        clientid => db.ClientSet.FirstOrDefault(c => c.Id == clientid)).ToList();
                //create
                var newTask = db.TaskSet.Create();
                //data
                newTask.Name = model.Name;
                newTask.Description = model.Description;
                newTask.DateBegin = model.DateBegin;
                newTask.DateEnd = model.DateEnd;
                newTask.PlannedDateEnd = model.PlannedDateEnd;
                newTask.PlannedTimeCost = model.PlannedTimeCost;
                newTask.Comments = model.Comments ?? String.Empty;
                //check
                //если автор не менеджер или если автор есть в исполнителях задачи
                //то надо проверить (это когда работники сами себе создают задачи)
                newTask.NeedToCheck = (!author.CompanyWorker.IsManager || workers.Contains(author));
                //type
                newTask.TaskType = db.TaskTypesSet.FirstOrDefault(tt => tt.Id == model.TypeId);
                //state
                var stateid = TaskStatesFormatter.ToInt(TaskStates.Created);
                newTask.TastState = db.TastStatesSet.FirstOrDefault(ts => ts.Id == stateid);
                //performers
                newTask.TaskPerformers = workers;
                //clients
                newTask.TaskClients = clients;
                //author
                newTask.TaskAuthor = author;
                //save
                db.TaskSet.Add(newTask);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    model.AddError(ex.Message);
                    throw;
                }
                
            }
            
            //var dd = model.DateBegin;
            //var ds = model.DateEndForDisplay;
            return RedirectToAction("Index");
        }

        /// <summary>
        /// get edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var curuser1 = WeMembership.GetCurrentUser();
            var model = new TaskEditModel();
            using (var db = new WeContext())
            {
                var task = db.TaskSet.FirstOrDefault(t => t.Id == id);
                if (task==null) return RedirectToAction("Index");

                var curuser = db.SiteUserSet.FirstOrDefault(s => s.Id == curuser1.Id);
                if (curuser == null) return RedirectToAction("Index");
                //can edit?
                if (!(curuser.IsAdmin || task.TaskAuthor.Id==curuser.Id || task.TaskPerformers.Contains(curuser)))
                {
                    //can not edit
                    return RedirectToAction("Index");
                }
                var performers = Common.GetAvailableClients(curuser);
                model.TaskTypes = db.TaskTypesSet.Select(tt => new TaskTypeModel
                    {
                        Id = tt.Id,
                        Name = tt.TypeName,
                        Description = tt.TypeDescription
                    }).ToList();
                model.StateId = TaskStatesFormatter.ToInt(TaskStates.Created);
                //model.TaskPerformers = performers;
                model.WorkersIdsNames = new Dictionary<string, int>();
                foreach (var siteUserModel in performers)
                {
                    model.WorkersIdsNames.Add(siteUserModel.GetFIO(), siteUserModel.Id);
                }
                model.ClientsIdsNames = new Dictionary<string, int>();
                foreach (var client in db.ClientSet.ToList())
                {
                    model.ClientsIdsNames.Add(client.GetFIO(), client.Id);
                }
                model.Id = task.Id;
                model.Name = task.Name;
                model.Description = task.Description;
                model.DateBegin = task.DateBegin;
                model.DateEnd = task.DateEnd;
                model.PlannedDateEnd = task.PlannedDateEnd??DateTime.Now;
                model.PlannedTimeCost = task.PlannedTimeCost??0;

                model.IsCurUserAuthor = (curuser.IsAdmin || task.TaskAuthor.Id == curuser.Id);
                model.TaskStates = db.TastStatesSet.Select(ts => new TaskStateModel
                    {
                        Id = ts.Id,
                        Name = ts.StateName,
                        Description = ts.StateDescription
                    }).ToList();
                var tx= task.TaskTimeCosts.Select(cost => new TaskCosts
                    {
                        DateTime = cost.DbDateTime.ToLongDateString(), 
                        Hours = Число.Пропись(cost.SpentHours, new ЕдиницаИзмерения(РодЧисло.Мужской, "час", "часа", "часов")), 
                        Spender = cost.TimeSpender.GetFIO(),
                        Date = cost.DbDateTime
                    }).ToList();

                model.TaskCosts = new List<TaskCosts>(tx.OrderByDescending(co => co.Date));
            }
            return View(model);
        }
        /// <summary>
        /// post edit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(TaskEditModel model)
        {
            var curuser1 = WeMembership.GetCurrentUser();
            using (var db = new WeContext())
            {
                var curuser = db.SiteUserSet.FirstOrDefault(s => s.Id == curuser1.Id);
                var author = db.SiteUserSet.FirstOrDefault(s => s.Id == curuser1.Id);
               
                var task = db.TaskSet.FirstOrDefault(t => t.Id == model.Id);
                if (task == null) return RedirectToAction("Index");
                 //selected workers
                var workers = task.TaskPerformers;
                
                
                //change timecost from model
                TaskTimeCost add = db.TaskTimeCostSet.Create();
                add.DbDateTime = DateTime.Now;
                add.SpentHours = model.TimeCostToAdd;
                add.Task = task;
                add.TimeSpender = curuser;
                db.TaskTimeCostSet.Add(add);
                db.SaveChanges();
                //
                //data
                task.Name = model.Name ?? string.Empty;
                task.Description = model.Description ?? string.Empty;
                task.DateBegin = model.DateBegin;
                task.DateEnd = model.DateEnd;
                task.PlannedDateEnd = model.PlannedDateEnd;
                task.PlannedTimeCost = model.PlannedTimeCost;
                task.Comments = model.Comments ?? String.Empty;
                //check
                //если автор не менеджер или если автор есть в исполнителях задачи
                //то надо проверить (это когда работники сами себе создают задачи)
                task.NeedToCheck = (!author.CompanyWorker.IsManager || workers.Contains(author));
                //type
                task.TaskType = db.TaskTypesSet.FirstOrDefault(tt => tt.Id == model.TypeId);
                //state
                var stateid = model.StateId;
                task.TastState = db.TastStatesSet.FirstOrDefault(ts => ts.Id == stateid);
                
                //task.TaskTimeCosts
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
