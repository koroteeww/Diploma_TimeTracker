using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebExplorer.Entity;
using WebExplorer.Models;
using WebExplorer.Models.ClientsDepartments;
using WebExplorer.Models.SiteUsers;
using WebExplorer.Models.Tasks;

namespace WebExplorer.Controllers
{
    public class SiteUsersController : Controller
    {
        //
        // GET: /SiteUsers/

        public ActionResult Index()
        {
            using (var db=new WeContext())
            {
                var model = new WebExplorer.Models.SiteUsers.SiteUsersListModel
                    {
                        Users = db.SiteUserSet
                                    .OrderBy(n => n.Login)
                                    .Select(
                                        n =>
                                        new SiteUserModel
                                        {

                                            Id = n.Id,
                                            Login = n.Login,
                                            Comment = n.Comment??String.Empty,
                                            Email = n.Email,
                                            IsAdmin = (n.IsAdmin)?"да":"нет",
                                            IsApproved = (n.IsApproved)?"да":"нет",
                                            IsLocked = (n.IsLocked)?"да":"нет",
                                            SiteUserRole = n.SiteUserRole.RoleDescription,
                                            SiteUserRoleId = n.SiteUserRole.Id,
                                            WorkerId = n.CompanyWorker.Id,
                                            WorkerName = n.CompanyWorker.LastName+" "+n.CompanyWorker.FirstName,
                                            CreationDate = n.CreationDate
                                        }
                        )
                                    .ToList()
                    };
                return View(model);
            }
            
        }
        /// <summary>
        /// get add
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add()
        {
            using (var db = new WeContext())
            {

                var rr = db.SiteUserRoleSet.ToList();
                List<SiteUserRoleModel> L = rr.Select(item => item.ToSiteUserRoleModel()).ToList();

                var model = new SiteUserAddModel
                {
                    Roles = db.SiteUserRoleSet.ToList().Select(item => item.ToSiteUserRoleModel()).ToList(),
                    Workers = db.ClientSet.Where(cl=>cl.SiteUser==null).ToList().Select(item => item.ToClientModel()).ToList()
                };
                return View(model);
            }


        }
        /// <summary>
        /// post add
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(SiteUserAddModel model)
        {
            using (var db = new WeContext())
            {

                var newUser = db.SiteUserSet.Create();
                if (model.WorkerId == null) return RedirectToAction("Index");
                var n = model;
                //соберем данные с модели

                newUser.Login = n.Login;
                newUser.Comment = n.Comment ?? String.Empty;
                newUser.Email = n.Email ?? String.Empty;
                newUser.IsAdmin = n.IsAdmin;
                newUser.IsApproved = true;
                newUser.IsLocked = false;
                newUser.SiteUserRole = db.SiteUserRoleSet.FirstOrDefault(r => r.Id == model.SiteUserRoleId);
                newUser.CompanyWorker = db.ClientSet.FirstOrDefault(cl => cl.Id == model.WorkerId);
                if (model.Pass != string.Empty)
                {
                    var salt = Auth.WeMembership.createSalt();
                    newUser.Password = Auth.WeMembership.createPasswordHash(model.Pass, salt);
                    newUser.Hash = salt;
                }
                


                newUser.CreationDate = DateTime.Now;
                db.SiteUserSet.Add(newUser);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// get edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult Edit(int? id)
        {
            using (var db = new WeContext())
            {
                var n = db.SiteUserSet.FirstOrDefault(t => t.Id == id);
                if (n == null) return RedirectToAction("Index");
                var model = new SiteUserAddModel
                    {
                        Id = n.Id,
                        Login = n.Login,
                        Comment = n.Comment ?? String.Empty,
                        Email = n.Email,
                        IsAdmin = n.IsAdmin,
                        IsApproved = n.IsApproved,
                        IsLocked = n.IsLocked,
                        SiteUserRole = n.SiteUserRole.RoleDescription,
                        SiteUserRoleId = n.SiteUserRole.Id,
                        WorkerId = n.CompanyWorker.Id,
                        WorkerName = n.CompanyWorker.LastName + " " + n.CompanyWorker.FirstName,
                        CreationDate = n.CreationDate,
                        Hash = String.Empty,
                        Pass = String.Empty,
                        Roles = db.SiteUserRoleSet.ToList().Select(role=>role.ToSiteUserRoleModel()).ToList(),
                        Workers = db.ClientSet.Where(cl => cl.SiteUser == null || cl.SiteUser.Id==id).ToList().Select(worker => worker.ToClientModel()).ToList()
                    };
                return View(model);

            }
            
        }

        /// <summary>
        /// post edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult Edit(SiteUserAddModel model)
        {
            using (var db = new WeContext())
            {
                
                var old = db.SiteUserSet.FirstOrDefault(ns => ns.Id == model.Id);
                
                var n = model;
                //соберем данные с модели
                
                old.Login = n.Login;
                old.Comment = n.Comment ?? String.Empty;
                old.Email = n.Email ?? String.Empty;
                old.IsAdmin = n.IsAdmin;
                old.IsApproved = n.IsApproved;
                old.IsLocked = n.IsLocked;
                old.SiteUserRole = db.SiteUserRoleSet.FirstOrDefault(r=>r.Id==model.SiteUserRoleId);
                old.CompanyWorker = db.ClientSet.FirstOrDefault(cl => cl.Id == model.WorkerId);
                if (model.Pass != string.Empty)
                {
                    var salt = Auth.WeMembership.createSalt();
                    old.Password = Auth.WeMembership.createPasswordHash(model.Pass, salt);
                    old.Hash = salt;
                }



                //old.CreationDate = n.CreationDate;

                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
