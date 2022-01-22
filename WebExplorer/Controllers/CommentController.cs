using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebExplorer.Auth;
using WebExplorer.Entity;
using WebExplorer.Helpers;
using WebExplorer.Models;

namespace WebExplorer.Controllers
{
    public class CommentController : Controller
    {
        /// <summary>
        /// Добавление комментария
        /// POST /Comment/Add
        /// </summary>
        /// <param name="id">Идентификатор новости</param>
        /// <param name="body">Текст комментария</param>
        [Authorize]
        [HttpPost]
        public ActionResult Add(int id, string body)
        {
            if (!TextHelper.IsEmptyText(body))
            {
                using (var db = new WeContext())
                {
                    //создаем
                    var authorId = WeMembership.GetCurrentUserId();
                    var newComment = db.NewsCommentsSet.Create();
                    newComment.Author = db.SiteUserSet.FirstOrDefault(u => u.Id == authorId);
                    newComment.News = db.NewsSet.FirstOrDefault(u => u.Id == id);
                    newComment.Body = body;
                    newComment.CreationDate = DateTime.Now;

                    db.NewsCommentsSet.Add(newComment);

                    db.SaveChanges();

                }
            }

            //на страницу новости
            return RedirectToAction("Detail", "News", new {id});
        }



        /// <summary>
        /// Сохранение (добавление или изменение) комментария
        /// POST /Comment/Edit
        /// </summary>
        /// <param name="id">Идентификатор комментария</param>
        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var db = new WeContext())
            {
                //редактируем коммент
                var authorId = WeMembership.GetCurrentUserId();
                //Админу можно редактировать всё                            
                var comment = db.NewsCommentsSet.FirstOrDefault(n => n.Id == id && (WeMembership.IsAdmin || n.Author.Id == authorId));

                //смотрим, нашли ли коммент
                if (comment == null || ( comment.Author.Id!=WeMembership.GetCurrentUserId()) || (!WeMembership.GetCurrentUser().IsAdmin ) )
                    return View(ErrorsModel.CreateWithError<EditCommentModel>("Не можем найти комментарий. Он точно ваш и его можно редактировать?"));

                //если нашли, редактируем
                return View(new EditCommentModel
                    {
                        Id = comment.Id,
                        Body = comment.Body
                    });
            }
        }

        /// <summary>
        /// Сохранение (добавление или изменение) комментария
        /// POST /Comment/Edit
        /// </summary>
        /// <param name="model">Модель комментария</param>
        [Authorize]
        [HttpPost]
        public ActionResult Edit(EditCommentModel model)
        {
            if (TextHelper.IsEmptyText(model.Body))
            {
                model.AddError("Нельзя сохранить пустой текст комментария");
                return View(model);
            }

            using (var db = new WeContext())
            {

                //редактируем коммент
                var authorId = WeMembership.GetCurrentUserId();
                //Админу можно редактировать всё                            
                var comment = db.NewsCommentsSet
                    .Include(c => c.News)
                    .FirstOrDefault(n => n.Id == model.Id && (WeMembership.IsAdmin || n.Author.Id == authorId));

                //смотрим, нашли ли коммент
                if (comment == null ||  ( comment.Author.Id!=WeMembership.GetCurrentUserId()) || (!WeMembership.GetCurrentUser().IsAdmin ) )
                    return RedirectToAction("Index", "News");

                //если нашли, редактируем
                comment.Body = model.Body;
                db.SaveChanges();

                return RedirectToAction("Detail", "News", new {id = comment.News.Id});
            }
        }
        /// <summary>
        /// список комментов сайта для админа
        /// просто коммент, к какой новости относится, и кнопки редактировать и удалить
        /// </summary>
        /// <returns></returns>
        public ActionResult ListAdmin()
        {
            return View();
        }
        /// <summary>
        /// удаление коммента
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            using (var db = new WeContext())
            {
                var userId = WeMembership.GetCurrentUserId();
                var comment = db.NewsCommentsSet
                    .FirstOrDefault(n => n.Id == id && (WeMembership.IsAdmin || n.Author.Id == userId));

                //смотрим, нашли ли
                if (comment == null)
                    return RedirectToAction("Index", "News");
                

                
                //удаляем
                db.NewsCommentsSet.Remove(comment);
                //сохраняемся изменения
                db.SaveChanges();

                //на главную
                return RedirectToAction("Index", "News");
            }
/*
            return RedirectToAction("Index", "News");
*/
        }
    }
}
