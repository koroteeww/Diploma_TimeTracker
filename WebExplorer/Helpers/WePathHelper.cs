using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace WebExplorer.Helpers
{
    /// <summary>
    /// Пути WebExploere
    /// </summary>
    public class WePathHelper
    {
        /// <summary>
        /// Путь для сохранения картинок на сервере
        /// </summary>
        public string ImageUploadServerPath { get; private set; }

        /// <summary>
        /// Путь до картинок на клиент
        /// </summary>
        public string ImageUploadVirtualPath { get; private set; }

        /// <summary>
        /// Корень эксплоререа
        /// </summary>
        public string RootPath { get; private set; }

        /// <summary>
        /// Список запрещенных расширений
        /// </summary>
        public IEnumerable<string> HideExtensions { get; private set; }

        /// <summary>
        /// Список запрещенных имен
        /// </summary>
        public IEnumerable<string> HideNames { get; private set; }

        /// <summary>
        /// Регулярка для проверки не скрыт ли файл
        /// </summary>
        public string LinkTemplate { get; private set; }

        /// <summary>
        /// Временный путь для загруженных файлов
        /// </summary>
        public string UploadTempPath { get; private set; }

        /// <summary>
        /// Символы разделения каталогов
        /// </summary>
        public readonly char[] DirectorySeparatorChars = new[] {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar}; 

        /// <summary>
        /// Для блокировки при создании синглтона
        /// </summary>
        private static readonly object SingletoneLock = new object();

        /// <summary>
        /// Инстанс синглтона
        /// </summary>
        private static volatile WePathHelper _instance;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        private WePathHelper()
        {
            ImageUploadVirtualPath = "/images";
            ImageUploadServerPath = HttpContext.Current.Server.MapPath("~/images");

            RootPath = ConfigurationManager.AppSettings["webExplorerRootPath"].TrimEnd(DirectorySeparatorChars);

            LinkTemplate = ConfigurationManager.AppSettings["webExplorerLinkTemplate"];
            if (!LinkTemplate.EndsWith("/"))
                LinkTemplate += "/";

            UploadTempPath = HttpContext.Current.Server.MapPath("~/App_Data/upload");

            var hideExpressions = ConfigurationManager.AppSettings["webExplorerHideMask"]
                .Trim(new[] {' ', '|'})
                .Split(new[] {'|'});

            HideExtensions = hideExpressions
                .Where(e => e.StartsWith("*."))
                .Select(e=>e.Substring(1));
            HideNames = hideExpressions
                .Where(e => !e.StartsWith("*."));
        }

        /// <summary>
        /// Текущая конфигурация
        /// </summary>
        public static WePathHelper Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (SingletoneLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new WePathHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Получает информацию о директории, если доступ к ней разрешен
        /// </summary>
        /// <param name="relativePath">Относительный путь</param>
        /// <returns>Если путь являетя допустимым, возвращает информацию о нем</returns>
        /// <exception cref="ArgumentException">Если путь неверный, или к нему нет доступа</exception>
        public static DirectoryInfo GetDirectoryInfoIfAllowed(string relativePath)
        {
            //уберем всякие html хрени
            relativePath = HttpUtility.UrlDecode(relativePath);

            //убедимся что путь существует
            var lookupPath = Path.Combine(Current.RootPath, (relativePath ?? String.Empty).Trim(Current.DirectorySeparatorChars));
            if (!Directory.Exists(lookupPath))
                throw new ArgumentException("Неверный путь");

            //и что он внутри заданного
            var info = new DirectoryInfo(lookupPath);
            if (!info.FullName.StartsWith(Current.RootPath))
                throw new ArgumentException("Неверный путь");

            return info;
        }
    }
}