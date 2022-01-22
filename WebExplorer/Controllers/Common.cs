using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebExplorer.Entity;
using WebExplorer.Models.SiteUsers;
using WebExplorer.Models.Tasks;

namespace WebExplorer.Controllers
{
    public static class Common
    {
        internal static List<Department> GetSubDepartments(Department root)
        {
            List<Department> ret = new List<Department>();
            ret.Add(root);
            foreach (var child in root.SubDepartments)
            {
                ret.AddRange(GetSubDepartments(child));
            }
            return ret;
        }
        /// <summary>
        /// get available task performers for this user
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal static List<SiteUser> GetAvailableClients(SiteUser user)
        {
            var ret = new List<SiteUser>();
            if (user.CompanyWorker.IsManager)
            {
                ret.Add(user);
                var curDpt = user.CompanyWorker.Department;
                //подчиненные отделы (корневой и все "дочки")
                var subDpr = GetSubDepartments(curDpt);
                var clientList = new List<Client>();
                using (var db = new WeContext())
                {
                    //идем и выбираем всех пользователей с этих отделов 
                    foreach (var dpt in subDpr)
                    {
                        var dpt1 = dpt;
                        clientList.AddRange(db.ClientSet.Where(c => c.Department.Id == dpt1.Id).ToList());
                    }

                    var clList = clientList.Where(client => client.SiteUser != null && client.Id != user.CompanyWorker.Id).ToList();
                    ret.AddRange(clList.Select(client => client.SiteUser));
                }
                return ret;
            }
            else
            {
                ret.Add(user);
            }
            return ret;
        }
        internal static List<TaskModelForList> addClientsPerformers(List<TaskModelForList> tasksModel)
        {
            using (var dd = new WeContext())
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
    }
    /// <summary>
    ///  Custom comparer
    /// </summary>
    internal class TaskModelForListComparer : IEqualityComparer<TaskModelForList>
    {

        public bool Equals(TaskModelForList x, TaskModelForList y)
        {

            //Check whether the compared objects reference the same data. 
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null. 
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether ids are equal
            return x.Id == y.Id;
        }

        // If Equals() returns true for a pair of objects  
        // then GetHashCode() must return the same value for these objects. 

        public int GetHashCode(TaskModelForList taskModel)
        {
            //Check whether the object is null 
            if (Object.ReferenceEquals(taskModel, null)) return 0;

            //Get hash code for the Name field if it is not null. 
            int hashName = taskModel.Name == null ? 0 : taskModel.Name.GetHashCode();

            //Get hash code for the Code field. 
            int hashCode = taskModel.Id.GetHashCode();

            //Calculate the hash code for the product. 
            return hashName ^ hashCode;
        }

    }
}