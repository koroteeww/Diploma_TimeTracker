using System;
using System.Web.Http;
using WebExplorer.Helpers;
using WebExplorer.Models;

namespace WebExplorer.Controllers
{
    public class DirectoryController : ApiController
    {
        /// <summary>
        /// Создание директории
        /// POST api/directory
        /// </summary>
        [Authorize]
        public void Post(DirectoryCreateModel request)
        {
            //получим ссылку на информацию о директории
            var info = WePathHelper.GetDirectoryInfoIfAllowed(request.Path);

            try
            {
                //попытаемся создать директрию
                info.CreateSubdirectory(request.Name);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Не удалось создать директорию", ex);
            }
        }
    }
}
