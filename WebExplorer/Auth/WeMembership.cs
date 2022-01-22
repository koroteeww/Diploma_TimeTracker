using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using WebExplorer.Entity;

namespace WebExplorer.Auth
{
    /// <summary>
    /// Кастомный провайдер авторизации
    /// </summary>
    public static class WeMembership
    {
        #region [ - Private members - ]
        /// <summary>
        /// Ключ для сохранения кэша текущего пользователя
        /// </summary>
        private const string CURRENT_USER_CACHE_KEY = "CurrentUser";

        /// <summary>
        /// Алгоритм хэширования
        /// </summary>
        private readonly static HashAlgorithm HashAlgorithm = new SHA256Managed();
        #endregion

        #region [ - Public methods - ]
        /// <summary>
        /// Идентификатор анонимного пользователя
        /// </summary>
        public const int AnonymousId = -1;

        /// <summary>
        /// Создание пользователя
        /// </summary>
        public static SiteUser CreateUser(string login, string password, string email, out MembershipCreateStatus status)
        {

            //валидация пользователя
            if (GetUserByEmail(email) != null)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            //проверка на повторы
            var u = GetUser(login);
            if (u != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            //сохранение
            try
            {
                using (var db = new WeContext())
                {
                    var salt = createSalt();
                    var newUser = new SiteUser
                        {
                            Login = login,
                            Password = createPasswordHash(password, salt),
                            Hash = salt,
                            Email = email,
                            LastLoginDate = DateTime.Now,
                            CreationDate = DateTime.Now,
                            IsApproved = true,
                            IsLocked = false,
                            IsAdmin = false
                        };
                    db.SiteUserSet.Add(newUser);
                    db.SaveChanges();

                    status = MembershipCreateStatus.Success;
                    return newUser;
                }
            }
            catch
            {
                status = MembershipCreateStatus.ProviderError;
                return null;
            }
        }

        /// <summary>
        /// Проверка, был ли пользователь авторизован
        /// </summary>
        public static SiteUser ValidateUser(string login, string password)
        {
            using (var db = new WeContext())
            {
                //поищем по логину и емэйлу
                var user = db.SiteUserSet
                    .FirstOrDefault(u => (u.IsApproved)
                                         && (!u.IsLocked)
                                         && (
                                                u.Login.Equals(login, StringComparison.OrdinalIgnoreCase)
                                                || u.Email.Equals(login, StringComparison.OrdinalIgnoreCase)
                                            ));

                //посмотрим нашлось что нибудь по логину
                if (user == null)
                    return null;

                //если проходит авторизация основным способом, то все ок
                if (user.Password == createPasswordHash(password, user.Hash))
                    return user;

                //посмотрим, может это старый юзер
                if(user.Hash=="md5" && user.Password == phpLikeMD5(password))
                {
                    user.Hash = createSalt();
                    user.Password = createPasswordHash(password, user.Hash);
                    db.SaveChanges();

                    return user;
                }

                //если ничего не подошло - null
                return null;
            }

        }

        /// <summary>
        /// Получение информации о пользователе по идентификатору
        /// </summary>
        public static SiteUser GetUser(int key)
        {
            using (var db = new WeContext())
            {
                return db.SiteUserSet.FirstOrDefault(u => u.Id == key);
            }
        }

        /// <summary>
        /// Получение информации о пользователе по логину
        /// </summary>
        public static SiteUser GetUser(string login)
        {
            using (var db = new WeContext())
            {
                return db.SiteUserSet.FirstOrDefault(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// Получение пользователя по email
        /// </summary>
        public static SiteUser GetUserByEmail(string email)
        {
            using (var db = new WeContext())
            {
                return db.SiteUserSet.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// Обновление пользователя
        /// </summary>
        public static MembershipCreateStatus UpdateUser(SiteUser user, string email, string password)
        {
            //проверим можно ли сохранить изменения
            if (email != user.Email)
            {
                var userByEmail = GetUserByEmail(email);
                if (userByEmail != null && userByEmail.Id != user.Id)
                    return MembershipCreateStatus.DuplicateEmail;
            }

            //сохранение
            using (var db = new WeContext())
            {
                db.SiteUserSet.Attach(user);

                user.Email = email;
                if (!String.IsNullOrEmpty(password))
                {
                    user.Hash = createSalt();
                    user.Password = createPasswordHash(
                        password,
                        user.Hash
                        );
                }

                db.SaveChanges();

                //инвалидация кэша
				//может не идти, если мы восстанавливаем пароль, тогда GetCurrentUser будет null
                if (GetCurrentUser() != null)
                {
                    if (user.Id == GetCurrentUserId())
                        HttpContext.Current.Items[CURRENT_USER_CACHE_KEY] = user;
                }
                return MembershipCreateStatus.Success;
            }
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        public static bool DeleteUser(string login, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        public static void LogOn(string login, bool remember = false)
        {
            FormsAuthentication.SetAuthCookie(login, remember);
        }

        /// <summary>
        /// Выход пользователя
        /// </summary>
        public static void LogOff()
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Пользователь авторизован
        /// </summary>
        public static bool IsAuthorized
        {
            get { return HttpContext.Current.User.Identity.IsAuthenticated; }
        }
        /// <summary>
        /// Пользователь админ
        /// </summary>
        public static bool IsAdmin
        {
            get 
            {
                if (!IsAuthorized)
				{
					return false;
				}
                
                
                var curUser = GetCurrentUser();
                //мало ли, вдруг null 
                if (curUser != null)
                {
                    //админ теперь напрямую из БД
                    return (curUser.IsAdmin);
                }
                return false;
            }
        }
        /// <summary>
        /// Пользователь менеджер
        /// </summary>
        public static bool IsManager
        {
            get
            {
                if (!IsAuthorized)
                {
                    return false;
                }


                var curUser = GetCurrentUser();
                //мало ли, вдруг null 
                if (curUser != null)
                {
                    using (var db=new WeContext() )
                    {
                        return db.ClientSet.Select(n => n.SiteUser.Id == curUser.Id && n.IsManager).Any();
                    }
                    
                    
                }
                return false;
            }
        }
        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public static SiteUser GetCurrentUser()
        {
            //кэш для каждого запроса
            var cache = HttpContext.Current.Items;

            //посмотрим, получали ли мы в этом контексте уже пользователя или нет
            if (cache.Contains(CURRENT_USER_CACHE_KEY))
                return (SiteUser)cache[CURRENT_USER_CACHE_KEY];

            //получим информацию из БД
            var currentUser = HttpContext.Current.User.Identity.IsAuthenticated
                                  ? GetUser(HttpContext.Current.User.Identity.Name)
                                  : null;

            //закэшируем
            if (currentUser != null)
                cache.Add(CURRENT_USER_CACHE_KEY, currentUser);
            else
            {
                //чтоб не возвращать null
                return new SiteUser { Id = AnonymousId ,IsAdmin = false, IsApproved = false, IsLocked = true, Login = "", Email="" };
                
            }
            return currentUser;
        }

        /// <summary>
        /// Идентификатор текущего пользователя
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentUserId()
        {
            if (!IsAuthorized)
                return AnonymousId;

            return GetCurrentUser().Id;
        }

        #endregion

        #region [ - Private methods - ]

        /// <summary>
        /// Возвращает хэш пароля, аналогичный phpшному md5
        /// </summary>
        private static string phpLikeMD5(string password)
        {
            byte[] textBytes = Encoding.Default.GetBytes(password);
            var cryptHandler = new MD5CryptoServiceProvider();
            byte[] hash = cryptHandler.ComputeHash(textBytes);
            var ret = new StringBuilder();
            foreach (byte a in hash)
                ret.Append(a.ToString("x2"));

            return ret.ToString();
        }

        /// <summary>
        /// Генерация соли для сохранения пароля в БД
        /// </summary>
        internal static string createSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[32];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        /// <summary>
        /// Хэширование пароля
        /// </summary>
        internal static string createPasswordHash(string pwd, string salt)
        {
            return Convert.ToBase64String(
                HashAlgorithm
                    .ComputeHash(
                        Encoding.Default.GetBytes(String.Concat(pwd, salt))
                    )
                );
        }

        #endregion
    }
}