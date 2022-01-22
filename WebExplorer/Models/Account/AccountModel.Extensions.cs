namespace WebExplorer.Models
{
    public static class AccountModelExtensions
    {
        /// <summary>
        /// Создает модель аккаунта на основе информации о пользователе
        /// </summary>
        /// <param name="user">Пользователь</param>
        public static AccountModel ToAccountModel(this WebExplorer.Entity.SiteUser user)
        {
            return new AccountModel
                {
                    Login = user.Login,
                    Email = user.Email
                };
        }
        /// <summary>
        /// Создает модель SiteUserModel
        /// </summary>
        /// <param name="user">Пользователь</param>
        public static SiteUserModel ToSiteUserModel(this WebExplorer.Entity.SiteUser user)
        {
            return new SiteUserModel
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                WorkerName = user.CompanyWorker.LastName+" "+user.CompanyWorker.FirstName+" "+user.CompanyWorker.MiddleName
            };
        }
    }
}