using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebExplorer.Auth;
using WebExplorer.Entity;
using WebExplorer.Models;
using WebExplorer.Models.ClientsDepartments;

namespace WebExplorer.Controllers
{
    public class ClientsController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new WeContext())
            {

                var curuser = WeMembership.GetCurrentUser();
                //список отделов
                var model = new ClientsListModel
                {

                    Clients = db.ClientSet
                                    .OrderBy(n => n.LastName)
                                    .Select(
                                        n =>
                                        new ClientModel
                                        {
                                            FirstName = n.FirstName,
                                            Id = n.Id,
                                            LastName = n.LastName,
                                            MiddleName = n.MiddleName ?? String.Empty,
                                            Location = n.Location,
                                            Position = n.Position ?? String.Empty,
                                            Company = n.Company,
                                            Phone = n.PhoneNumber ?? String.Empty,
                                            IsManager = n.IsManager,
                                            Comment = n.Comment ?? String.Empty,
                                            DepartmentId = n.Department.Id,
                                            DepartmentName = n.Department.Name,
                                            IsManagerString = (n.IsManager)?"Да":"Нет"
                                        }
                        )
                                    .ToList()
                };
                return View(model);
            }
            
        }
        /// <summary>
        /// добавление сотрудника
        /// GET: /Clients/Add
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            using (var db = new WeContext())
            {
                var existingDptSet = db.DepartmentSet.OrderBy(n => n.Name).ToList();
                List<DepartmentModel> existingDpt = new List<DepartmentModel>();
                //ex.Add(null);
                foreach (var department in existingDptSet)
                {
                    existingDpt.Add(new DepartmentModel
                        {
                            Id = department.Id,
                            Name = department.Name,
                            ParentId = (department.ParentDepartment == null) ? -1 : department.ParentDepartment.Id,
                            ParentName =
                                (department.ParentDepartment == null) ? String.Empty : department.ParentDepartment.Name
                        });
                }
                return View("Add", new ClientsAddModel
                {
                    ExistingDepartments = existingDpt,
                    Company = @"ОАО «ВПК НПО машиностроения»"
                });
            }
        }
        /// <summary>
        /// добавление отдела
        /// POST: /Dpt/Add
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult Add(ClientsAddModel model)
        {
            //сохраним если можем
            using (var db = new WeContext())
            {


                //создаем
                var clientNew = db.ClientSet.Create();
                //заполняем
                clientNew.FirstName = model.FirstName;
                //FirstName = model.FirstName;
                clientNew.LastName = model.LastName;
                clientNew.MiddleName = model.MiddleName ?? string.Empty;
                clientNew.Location = model.Location;
                clientNew.Position = model.Position ?? String.Empty;
                clientNew.Company = model.Company;
                clientNew.PhoneNumber = model.Phone ?? String.Empty;
                clientNew.IsManager = model.IsManager;
                clientNew.Comment = model.Comment ?? String.Empty;
                var dp = db.DepartmentSet.FirstOrDefault(d => d.Id == model.DepartmentId);
                clientNew.Department = dp;
                
                
                //сохраняем
                db.ClientSet.Add(clientNew);

                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// редактирование сотрудника
        /// Get Clients/Edit/Id
        /// </summary>
        /// <param name="id">ид</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var db = new WeContext())
            {
                var existingDptSet= db.DepartmentSet.OrderBy(n => n.Name).ToList();
                List<DepartmentModel> existingDpt = new List<DepartmentModel>();
                //ex.Add(null);
                foreach (var department in existingDptSet)
                {
                    existingDpt.Add(new DepartmentModel
                        {
                            Id = department.Id,
                            Name = department.Name,
                            ParentId = (department.ParentDepartment == null) ? -1 : department.ParentDepartment.Id,
                            ParentName = (department.ParentDepartment == null) ? String.Empty : department.ParentDepartment.Name
                        });
                }
                var existingClient = db.ClientSet.FirstOrDefault(n => n.Id == id);
                if (existingClient == null)
                    return RedirectToAction("Index");

                return View("Add", new ClientsAddModel
                    {
                        Id = existingClient.Id, 
                        FirstName = existingClient.FirstName, 
                        LastName = existingClient.LastName,
                        MiddleName = existingClient.MiddleName ?? string.Empty,
                        Location = existingClient.Location,
                        Position = existingClient.Position??String.Empty,
                        Company = existingClient.Company,
                        Phone = existingClient.PhoneNumber??String.Empty,
                        IsManager = existingClient.IsManager,
                        Comment = existingClient.Comment??String.Empty,
                        DepartmentId = existingClient.Department.Id,
                        ExistingDepartments = existingDpt
                    });
            }
        }
        /// <summary>
        /// редактирование департамента
        /// POST Dpt/Edit/Id
        /// </summary>
        /// <param name="id">ид</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(ClientsAddModel model)
        {
            using (var db = new WeContext())
            {
                //полуим
                var clientRedact = db.ClientSet.FirstOrDefault(cl => cl.Id == model.Id);
                //заполняем из модели
                clientRedact.FirstName = model.FirstName;
                //FirstName = model.FirstName;
                clientRedact.LastName = model.LastName;
                clientRedact.MiddleName = model.MiddleName ?? string.Empty;
                clientRedact.Location = model.Location;
                clientRedact.Position = model.Position ?? String.Empty;
                clientRedact.Company = model.Company;
                clientRedact.PhoneNumber = model.Phone ?? String.Empty;
                clientRedact.IsManager = model.IsManager;
                clientRedact.Comment = model.Comment ?? String.Empty;
                var dp = db.DepartmentSet.FirstOrDefault(d => d.Id == model.DepartmentId);
                clientRedact.Department = dp;


                //сохраняем
                db.SaveChanges();



            }
            return RedirectToAction("Index");
        }
    }
}