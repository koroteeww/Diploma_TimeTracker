using System;
using WebExplorer.Auth;
using WebExplorer.Entity;
using WebExplorer.Helpers;

namespace WebExplorer.Models
{
    public static class DepatmentModelExtensions
    {
        /// <summary>
        /// Создает модель комментария на основе комментария из БД
        /// </summary>
        /// <param name="comment">Комментарий из БД</param>
        /// <returns>Модель комментария</returns>
        public static DepartmentModel ToDptModel(this  Department department)
        {

            return new DepartmentModel
            {
                Id = department.Id,Name = department.Name,
                ParentId = (department.ParentDepartment == null) ? -1 : department.ParentDepartment.Id,
                ParentName = (department.ParentDepartment == null) ? String.Empty : department.ParentDepartment.Name
                
            };
        }
    }
}