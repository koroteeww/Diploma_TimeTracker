using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebExplorer.Auth;
using WebExplorer.Entity;
using WebExplorer.Helpers;
using WebExplorer.Models;

namespace WebExplorer.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Профиль пользователя
        /// GET: /Profile/
        /// </summary>
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return View(WeMembership.GetCurrentUser().ToAccountModel());
        }

        /// <summary>
        /// Профиль пользователя
        /// POST: /Profile/
        /// </summary>
        [Authorize]
        [HttpPost]
        public ActionResult Index(AccountModel model)
        {
            //если все ок - сохраняем результат
            if(ModelState.IsValid)
            {
                //Обновним пользователя
                var result = WeMembership.UpdateUser(
                    WeMembership.GetCurrentUser(),
                    model.Email,
                    model.Password
                    );

                //посмотрим что получилось
                switch (result)
                {
                 case MembershipCreateStatus.Success:
                        break;
                    case MembershipCreateStatus.DuplicateEmail:
                        ModelState.AddModelError("Email","Этот ящик уже занят.");
                        break;
                    default:
                        model.AddError("Кажется, вы пытаетесь нас обмануть.");
                        break;
                }
            }

            if (!ModelState.IsValid)
                model.AddError("Кажется, вы пытаетесь нас обмануть.");

            return View(model);
        }

        /// <summary>
        /// Авторизация
        /// GET: /Profile/LogOn
        /// </summary>
        [HttpGet]
        public ActionResult LogOn(string returnUrl)
        {
            //только для тех кто не авторизован
            if (WeMembership.IsAuthorized)
                RedirectToAction("Index", "News");

            return View(new LogOnModel {ReturnUrl = returnUrl});
        }

        /// <summary>
        /// Авторизация
        /// POST: /Profile/LogOn
        /// </summary>
        [HttpPost]
        public ActionResult LogOn(LogOnModel model)
        {
            //только для тех кто не авторизован
            if (WeMembership.IsAuthorized)
                RedirectToAction("Index", "News");

            //авторизуемся
            var user = WeMembership.ValidateUser(model.Login, model.Password);
            if (user != null)
            {
                WeMembership.LogOn(user.Login, model.Remember);
                if (!String.IsNullOrEmpty(model.ReturnUrl) && model.ReturnUrl.StartsWith("/") && !model.ReturnUrl.StartsWith("//"))
                    return Redirect(model.ReturnUrl);

                return RedirectToAction("Index", "News");
            }

            //ошибка авторизации
            model.AddError("Ошибка авторизации. Неверный логин\\email или пароль?");

            return View(model);
        }

        /// <summary>
        /// Де-Авторизация
        /// GET: /Profile/LogOff
        /// </summary>
        [Authorize]
        [HttpGet]
        public ActionResult LogOff()
        {
            WeMembership.LogOff();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Восттановление пароля
        /// GET Account/PasswordRecovery
        /// </summary>
        [HttpGet]
        public ActionResult PasswordRecovery()
        {
            //возвращаем модель для ввода е-майла
            return View(new PasswordRecoveryModel());
        }

        /// <summary>
        /// Восттановление пароля
        /// POST Account/PasswordRecovery
        /// </summary>
        [HttpPost]
        public ActionResult PasswordRecovery(PasswordRecoveryModel model)
        {
            //если в модели ошибки, то выходим
            if (!ModelState.IsValid)
                return View(model);

            //генерация нового хеша и отправка его юзверю
            var user = WeMembership.GetUserByEmail(model.Email);
            if (user == null)
            {
                model.AddError("Пользователя с таким е-мейл нет в базе!");
                return View(model);
            }
            //формируем хеш для восстановления ссылки                  
            var hash = Guid.NewGuid().ToString();

            //пишем сформированный хеш в базу
            using (var db = new WeContext())
            {
                var dbPass = db.PasswordRecoverSet.Create();
                dbPass.CreationDate = DateTime.Now;
                dbPass.Email = model.Email;
                dbPass.Hash = hash;
                db.PasswordRecoverSet.Add(dbPass);
                db.SaveChanges();
            }

            //отправляем юзеру е-мейл с хешем
            var body = @"Для восстановления пароля на сайте перейдите по следующей ссылке: http://ak-5.ru/Account/PasswordUpdate/?hash=" + hash;
            const string subject = "Восстановление пароля на ak-5.ru";
            EmailHelper.SendEmail(user.Email, subject, body);
            model.LetterSent = true;

            //все успешно выведем сообщение
            return View(model);
        }

        /// <summary>
        /// Страничка для ввода нового пароля
        /// </summary>
        [HttpGet]
        public ActionResult PasswordUpdate(string hash)
        {
            //извлекаем запись про хеш
            using (var db = new WeContext())
            {
                //проверяем хэш
                var dbhash = db.PasswordRecoverSet.FirstOrDefault(pass => pass.Hash == hash);
                if (dbhash == null)
                    return View(ErrorsModel.CreateWithError<PasswordUpdateModel>("Хэш не найден. Попробуйте еще раз. У вас обязательно все получится."));

                //проверяем что хеш еще не истек
                if ((DateTime.Now - dbhash.CreationDate).TotalDays > 3)
                    return View(ErrorsModel.CreateWithError<PasswordUpdateModel>("Это письмо восстановления уже совсем старое и местами заплесневело. Попробуйте еще раз."));

                //показываем юзеру страницу, где он может ввести себе новый пароль
                var result = new PasswordUpdateModel {Hash = hash, HashValid = true};
                return View(result);
            }
        }

        /// <summary>
        /// Восттановление пароля
        /// POST Account/PasswordRecovery
        /// </summary>
        [HttpPost]
        public ActionResult PasswordUpdate(PasswordUpdateModel model)
        {
            PreStartApp.logger.Info("pass update");

            //у нас идет восстановление пароля
            using (var db = new WeContext())
            {
                var dbhash = db.PasswordRecoverSet.FirstOrDefault(pass => pass.Hash == model.Hash);

                //если что-то не так, идем на главную
                if (dbhash == null)
                    return RedirectToAction("Index");

                //получаем юзера по мылу, связанному с хешом.
                var user = WeMembership.GetUserByEmail(dbhash.Email);
                if (user == null)
                    return RedirectToAction("Index");

                //обновляем пароль
                WeMembership.UpdateUser(user, user.Email, model.NewPassword);

                //удаляем хэш (второй раз использовать нельзя)
                db.PasswordRecoverSet.Remove(dbhash);

                //если юезр только что установл пароль, то очевидно надо его сразу и залогинить?
                WeMembership.LogOn(user.Login, true);

                //пароль обновлен, идем на главную
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// TODO: тут будет регистрация...
        /// </summary>
        public ActionResult Register()
        {
            return View();
        }
        /*
        //
        // GET: /Profile/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Profile/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Profile/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Profile/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Profile/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Profile/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Profile/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        */
    }
}
