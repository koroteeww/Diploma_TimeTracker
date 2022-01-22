using System.Web.Mvc;

namespace WebExplorer.Controllers
{
    public class ExplorerController : Controller
    {
        /// <summary>
        /// Страница списка файлов
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}
