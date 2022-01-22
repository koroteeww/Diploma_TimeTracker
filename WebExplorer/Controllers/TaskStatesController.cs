using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebExplorer.Auth;
using WebExplorer.Entity;
using WebExplorer.Models;
using WebExplorer.Models.Tasks;

namespace WebExplorer.Controllers
{
    public class TaskStatesController : Controller
    {
        //
        // GET: /TaskTypes/

        public ActionResult Index()
        {
            using (var db = new WeContext())
            {

                var curuser = WeMembership.GetCurrentUser();
                //список отделов
                var model = new TaskStatesListModel
                {

                    TaskStates = db.TastStatesSet
                                    .OrderBy(n => n.StateName)
                                    .Select(
                                        n =>
                                        new TaskStateModel
                                        {
                                            
                                            Id = n.Id,
                                            Name = n.StateName,
                                            Description = n.StateDescription??String.Empty
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
        public ActionResult Edit(TaskStateModel model)
        {
            using (var db = new WeContext())
            {
                var ex = db.TastStatesSet.FirstOrDefault(t => t.Id == model.Id);
                ex.StateName= model.Name;
                ex.StateDescription = model.Description;
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
            return View(new TaskStateModel());
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
                var ex = db.TastStatesSet.FirstOrDefault(t => t.Id == id);
                if (ex == null) return RedirectToAction("Index");
                return View("Add",new TaskStateModel
                    {
                        Name = ex.StateName,
                        Description = ex.StateDescription??String.Empty,
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
        public ActionResult Add(TaskStateModel model)
        {
            using (var db = new WeContext())
            {
                var ex = db.TastStatesSet.Create();
                ex.StateName = model.Name;
                ex.StateDescription = model.Description;
                db.TastStatesSet.Add(ex);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
            
        }
    }

    }

