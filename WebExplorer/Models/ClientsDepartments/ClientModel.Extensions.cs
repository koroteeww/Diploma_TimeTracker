using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebExplorer.Entity;

namespace WebExplorer.Models.ClientsDepartments
{
    public static class ClientModelExtensions
    {
        /// <summary>
        /// Создает модель комментария на основе комментария из БД
        /// </summary>
        /// <param name="comment">Комментарий из БД</param>
        /// <returns>Модель комментария</returns>
        public static ClientModel ToClientModel(this  Client n)
        {

            return new ClientModel
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
                IsManagerString = (n.IsManager) ? "Да" : "Нет"
            };
        }
        public static string GetFIO(this ClientModel client)
        {
            return client.LastName + " " + client.FirstName + " " + client.MiddleName;
        }
    }
}