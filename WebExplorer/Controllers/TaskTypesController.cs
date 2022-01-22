using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebExplorer.Auth;
using WebExplorer.Entity;
using WebExplorer.Models;
using WebExplorer.Models.ClientsDepartments;
using WebExplorer.Models.Tasks;

namespace WebExplorer.Controllers
{
    public class TaskTypesController : Controller
    {
        //
        // GET: /TaskTypes/

        public ActionResult Index()
        {
            using (var db = new WeContext())
            {

                var curuser = WeMembership.GetCurrentUser();
                //список отделов
                var model = new TaskTypesListModel
                {

                    TaskTypes = db.TaskTypesSet
                                    .OrderBy(n => n.TypeName)
                                    .Select(
                                        n =>
                                        new TaskTypeModel
                                        {
                                            
                                            Id = n.Id,
                                            Name = n.TypeName,
                                            Description = n.TypeDescription??String.Empty
                                        }
                        )
                                    .ToList()
                };
                return View(model);
            }
        }
        /// <summary>
        /// post edit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult Edit(TaskTypeModel model)
        {
            using (var db = new WeContext())
            {
                var ex = db.TaskTypesSet.FirstOrDefault(t => t.Id == model.Id);
                ex.TypeName = model.Name;
                ex.TypeDescription = model.Description;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// get add
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            return View(new TaskTypeModel());
        }
        /// <summary>
        /// get edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            using (var db = new WeContext())
            {
                var ex = db.TaskTypesSet.FirstOrDefault(t => t.Id == id);
                if (ex == null) return RedirectToAction("Index");
                return View("Add",new TaskTypeModel
                    {
                        Name = ex.TypeName,
                        Description = ex.TypeDescription??String.Empty,
                        Id = ex.Id
                    });
            }
            
        }
        /// <summary>
        /// post add
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult Add(TaskTypeModel model)
        {
            using (var db = new WeContext())
            {
                var ex = db.TaskTypesSet.Create();
                ex.TypeName = model.Name;
                ex.TypeDescription = model.Description;
                db.TaskTypesSet.Add(ex);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
            
        }
    }
}
