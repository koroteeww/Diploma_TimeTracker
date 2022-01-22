using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebExplorer.Auth;
using WebExplorer.Entity;
using WebExplorer.Helpers;
using WebExplorer.Models;
using WebExplorer.Models.ClientsDepartments;

namespace WebExplorer.Controllers
{
    public class DepartmentsController : Controller
    {
        //
        // GET: /Departments/

        public ActionResult Index()
        {
            using (var db = new WeContext())
            {

                var curuser = WeMembership.GetCurrentUser();
                //список отделов
                var model = new DepartmentTreeModel
                    {

                        Departments = db.DepartmentSet
                                        .OrderBy(n => n.Name)
                                        .Select(
                                            n =>
                                            new DepartmentModel
                                                {
                                                    Name = n.Name,
                                                    Id = n.Id,
                                                    ParentId = n.ParentDepartment.Id,
                                                    ParentName=n.ParentDepartment.Name
                                                }
                            )
                                        .ToList()
                    };
                return View(model);
            }
        }
        /// <summary>
        /// добавление отдела
        /// GET: /Dpt/Add
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            using (var db=new WeContext())
            {
                var existing = db.DepartmentSet.OrderBy(n=>n.Name).ToList();
                List<DepartmentModel> ex = new List<DepartmentModel>();
                ex.Add(null);
                foreach (var department in existing)
                {
                    ex.Add(new DepartmentModel
                        {
                            Id = department.Id,
                            Name = department.Name,
                            ParentId = (department.ParentDepartment==null)?-1:department.ParentDepartment.Id,
                            ParentName = (department.ParentDepartment == null) ? String.Empty : department.ParentDepartment.Name
                        });
                }

                return View("Add", new DepartmentAddModel { ExistingDepartments = ex });
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
        public ActionResult Add(DepartmentAddModel model)
        {
            //сохраним если можем
                using (var db = new WeContext())
                {
                    

                    //создаем
                    var dpt = db.DepartmentSet.Create();
                    dpt.Name = model.Name;
                    //здесь-то всё отлично пашет...
                    dpt.ParentDepartment = model.ParentId == null ? null : db.DepartmentSet.FirstOrDefault(dept => dept.Id == model.ParentId);
                    db.DepartmentSet.Add(dpt);

                    db.SaveChanges();
                    
                }
                return RedirectToAction("Index");
        }
        /// <summary>
        /// редактирование департамента
        /// Get Dpt/Edit/Id
        /// </summary>
        /// <param name="id">ид</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            using (var db = new WeContext())
            {
                var existing = db.DepartmentSet.OrderBy(n => n.Name).ToList();
                List<DepartmentModel> ex = new List<DepartmentModel>();
                ex.Add(null);
                foreach (var department in existing)
                {
                    //сам на себя чтоб не ссылался
                    if (department.Id!=id)
                        ex.Add(new DepartmentModel
                        {
                            Id = department.Id,
                            Name = department.Name,
                            ParentId = (department.ParentDepartment == null) ? -1 : department.ParentDepartment.Id,
                            ParentName = (department.ParentDepartment == null) ? String.Empty : department.ParentDepartment.Name
                        });
                }
                var dd = db.DepartmentSet.FirstOrDefault(d => d.Id == id);
                string name = (dd==null)?String.Empty:dd.Name;

                DepartmentModel parent = null;
                if (dd!=null && dd.ParentDepartment != null)
                    parent = db.DepartmentSet.FirstOrDefault(dpt=>dpt.Id==dd.ParentDepartment.Id).ToDptModel();
                int? parentId = null;
                if (parent != null) parentId = parent.Id;

                return View("Add", new DepartmentAddModel { Id = id, Name = name, ExistingDepartments = ex, ParentId = parentId });
            }
        }
        /// <summary>
        /// редактирование департамента
        /// POST Dpt/Edit/Id
        /// </summary>
        /// <param name="id">ид</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult Edit(DepartmentAddModel model)
        {
            using (var db = new WeContext())
            {

                
                //редактируем
                
                Department dbDpt = db.DepartmentSet.FirstOrDefault(n => n.Id == model.Id.Value);

                //смотрим, нашли ли
                if (dbDpt == null)
                {
                    model.AddError("Id is null");
                    return View("Add", model);
                }
                //редактим
                dbDpt.Name = model.Name;
                if (model.ParentId == null)
                {
                    //TODO: what the FUCK?????????
                    //работает через раз или вообще никак
                    
                    Department dbNull = null;
                    dbDpt.ParentDepartment = dbNull;
                    dbDpt.ParentDepartment = dbNull;
                    dbDpt.ParentDepartment = dbNull;
                    dbDpt.ParentDepartment = dbNull;
                    dbDpt.ParentDepartment = dbNull;
                    dbDpt.ParentDepartment = dbNull;
                    
                    //string q = String.Format("UPDATE dbo.DepartmentSet SET ParentDepartment_Id=NULL WHERE Id={0}", dbDpt.Id);

                    //db.DepartmentSet.SqlQuery(q);
                }
                else
                {
                    dbDpt.ParentDepartment = db.DepartmentSet.FirstOrDefault(dept => dept.Id == model.ParentId);
                }

                

                db.SaveChanges();
                //db.DepartmentSet.
                
                
                

                
                
            }
            return RedirectToAction("Index");
        }
    }
}
