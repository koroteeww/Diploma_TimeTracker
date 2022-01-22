using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebExplorer.Entity;

namespace WebExplorer.Models
{
    public static class UserModelExtensions
    {
        /// <summary>
        ///  Конструктор на основе информации о пользователе
        /// </summary>
        /// <param name="user"></param>
        public static UserModel ToUserModel(this SiteUser user)
        {
            return new UserModel
                {
                    Login = user.Login
                };
        }
    }
}