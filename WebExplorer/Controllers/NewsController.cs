using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using WebExplorer.Auth;
using WebExplorer.Entity;
using WebExplorer.Helpers;
using WebExplorer.Models;
namespace WebExplorer.Controllers
{
    public class NewsController : Controller
    {
        //
        // GET: /News/

        /// <summary>
        /// Главная страница новостей
        /// </summary>
        public ActionResult Index(int id = 1)
        {
            using (var db = new WeContext())
            {
                var page = Math.Max(id, 1);
                var newsCount = db.NewsSet.Count();
                var curuser = WeMembership.GetCurrentUser();
                //список новостей
                var model = new NewsListModel
                {
                    TotalNewsCount = newsCount,
                    CurrentPage = page,
                    TotalPagesCount = (int)Math.Ceiling(newsCount / (float)10),
                    News = db.NewsSet
                        .OrderByDescending(n => n.CreationDate)
                        .Skip((page - 1) * 10)
                        .Take(10)
                        .Include(n => n.Author)
                        .Include(n => n.NewsComments)
                        .Select(n => new
                        {
                            Item = n,
                            CommentCount = n.NewsComments.Count
                        }
                        )
                        .ToList()
                        .Select(
                            n =>
                            new NewsModel
                            {
                                Title = n.Item.Title,
                                Id = n.Item.Id,
                                Author = n.Item.Author.Login,
                                AuthorId = n.Item.Author.Id,
                                Date = TextHelper.PrepareForDisplay(n.Item.CreationDate),
                                Body = TextHelper.PrepareForDisplay(TextHelper.CutBody(n.Item.Body)),
                                CommentsCount = n.CommentCount,
                                CanEdit = (curuser.Id == n.Item.Id || curuser.IsAdmin)
                            }
                        )
                        .ToList()
                };

                return View(model);
            }
        }
        /// <summary>
        /// Детальная информация о новости
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            using (var db = new WeContext())
            {
                var curuser = WeMembership.GetCurrentUser();
                //получим список новостей
                var detail = db.NewsSet
                    .Where(n => n.Id == id)
                    .Include(n => n.NewsComments)
                    .Include(n => n.Author)
                    .Include(n => n.NewsComments.Select(c => c.Author))
                    .Take(1)
                    .ToList()
                    .Select(
                        n =>
                        new NewsDetailModel
                        {
                            Title = n.Title,
                            Id = n.Id,
                            Author = n.Author.Login,
                            AuthorId = n.Author.Id,
                            Date = TextHelper.PrepareForDisplay(n.CreationDate),
                            Body = TextHelper.PrepareForDisplay(n.Body),
                            CommentsCount = n.NewsComments.Count,
                            CanEdit = (n.Id == curuser.Id || curuser.IsAdmin),
                            Comments = n.NewsComments
                                .OrderBy(c => c.CreationDate)
                                .Select(c => c.ToNewsCommentModel())
                        }
                    )
                    .FirstOrDefault();

                return View(detail);
            }
        }
        /// <summary>
        /// Добавление новости
        /// GET: /Home/Edit
        /// </summary>
        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            return View("Edit", new NewsEditModel());
        }

        /// <summary>
        /// Добавление или редактирование или удаление новости
        /// POST: /Home/Edit
        /// </summary>
        [HttpPost]
        [Authorize]
        public ActionResult Add(NewsEditModel model)
        {
            if (TextHelper.IsEmptyText(model.Body))
                model.AddError("Помоему, текст новости пустой");

            if (TextHelper.IsEmptyText(model.Title))
                model.AddError("Помоему, текст заголовка пустой");

            //обработаем ошибки
            if (!ModelState.IsValid || model.Errors.Any())
            {
                foreach (var error in ModelState.Values.Where(x => x.Errors.Count >= 1).SelectMany(m => m.Errors).Select(e => e.ErrorMessage))
                    model.AddError(error);
                return View("Edit", model);
            }

            try
            {
                //сохраним новость, если можем
                using (var db = new WeContext())
                {
                    var authorId = WeMembership.GetCurrentUserId();

                    //создаем
                    var newNews = db.NewsSet.Create();
                    newNews.CreationDate = DateTime.Now;
                    newNews.Author = db.SiteUserSet.FirstOrDefault(u => u.Id == authorId);
                    newNews.Title = model.Title;
                    newNews.Body = model.Body;

                    db.NewsSet.Add(newNews);

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                model.AddError(ex.Message);
                return View("Edit", model);
            }
        }

        /// <summary>
        /// Редактирование новости
        /// GET: /Home/Edit
        /// </summary>
        [HttpGet]
        [Authorize]
        public ActionResult Edit(int? id)
        {
            //если редактирование, получим модель новости
            using (var db = new WeContext())
            {
                //получим модель
                var authorId = WeMembership.GetCurrentUserId();
                //админ может редактировать всё
                var newsModel = db.NewsSet
                    .Where(n => n.Id == id.Value && (WeMembership.IsAdmin || n.Author.Id == authorId))
                    .ToList()
                    .Select(n => new NewsEditModel
                    {
                        Id = n.Id,
                        Title = n.Title,
                        Body = n.Body,
                    })
                    .FirstOrDefault();
                //если не нашли
                if (newsModel == null)
                {
                    newsModel = new NewsEditModel();
                    newsModel.AddError("Новость не найдена, а может вы пытаетесь редактировать чужую новость или её редактировать нельзя?");
                }

                return View(newsModel);
            }
        }


        /// <summary>
        /// Добавление или редактирование или удаление новости
        /// POST: /Home/Edit
        /// </summary>
        [HttpPost]
        [Authorize]
        public ActionResult Edit(NewsEditModel model)
        {
            if (TextHelper.IsEmptyText(model.Body))
                model.AddError("Помоему, текст новости пустой");

            if (TextHelper.IsEmptyText(model.Title))
                model.AddError("Помоему, текст заголовка пустой");

            //обработаем ошибки
            if (!ModelState.IsValid || model.Errors.Any())
            {
                foreach (var error in ModelState.Values.Where(x => x.Errors.Count >= 1).SelectMany(m => m.Errors).Select(e => e.ErrorMessage))
                    model.AddError(error);
                return View(model);
            }

            try
            {
                //сохраним новость, если можем
                using (var db = new WeContext())
                {
                    var authorId = WeMembership.GetCurrentUserId();

                    //редактируем
                    var dbNews = db.NewsSet.FirstOrDefault(n => n.Id == model.Id.Value && (WeMembership.IsAdmin || n.Author.Id == authorId));

                    //смотрим, нашли ли
                    if (dbNews == null || (dbNews.Author.Id != WeMembership.GetCurrentUserId()) || (!WeMembership.GetCurrentUser().IsAdmin))
                        return View(ErrorsModel.CreateWithError<NewsEditModel>("Новость не найдена, а может вы пытаетесь редактировать чужую новость?"));

                    //правим
                    dbNews.Title = model.Title;
                    dbNews.Body = model.Body;

                    db.SaveChanges();
                    //идем на главную
                    return RedirectToAction("Detail", new { dbNews.Id });

                }
            }
            catch (Exception ex)
            {
                model.AddError(ex.Message);
                return View(model);
            }
        }

        /// <summary>
        /// Удаление новости
        /// Get /Home/Delete/5
        /// </summary>
        /// <param name="id">Идентификатор удаляемой новости</param>
        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var db = new WeContext())
            {
                var userId = WeMembership.GetCurrentUserId();
                var news = db.NewsSet
                    .FirstOrDefault(n => n.Id == id && (WeMembership.IsAdmin || n.Author.Id == userId));

                //смотрим, нашли ли
                if (news == null)
                    return RedirectToAction("Index");

                return View(news);
            }
        }

        /// <summary>
        /// Удаление новости
        /// Get /Home/Delete/5
        /// </summary>
        /// <param name="id">Идентификатор удаляемой новости</param>
        /// <param name="confirm">Подтверждение об удалении новости</param>
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id, bool confirm)
        {
            using (var db = new WeContext())
            {
                var userId = WeMembership.GetCurrentUserId();
                var news = db.NewsSet
                    .FirstOrDefault(n => n.Id == id && (WeMembership.IsAdmin || n.Author.Id == userId));

                //смотрим, нашли ли
                if (news == null)
                    return RedirectToAction("Index");
                var comments = db.NewsCommentsSet.Where(comm=>comm.News.Id==news.Id);

                //удалим сначала комменты
                foreach (var comment in comments)
                {
                    db.NewsCommentsSet.Remove(comment);
                }
                //удаляем теперь новость
                db.NewsSet.Remove(news);
                //сохраняемся изменения
                db.SaveChanges();

                //на главную
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// список новостей сайта для админа
        /// просто новость-заголовок (без текста) и кнопки редактировать и удалить
        /// </summary>
        /// <returns></returns>
        public ActionResult ListAdmin()
        {
            return View();
        }
    }
}
