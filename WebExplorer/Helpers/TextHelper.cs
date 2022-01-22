using System.Globalization;
using System.Web.Mvc;
using CodeKicker.BBCode;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace WebExplorer.Helpers
{
    /// <summary>
    /// Операции с текстом
    /// </summary>
    public static class TextHelper
    {
        #region [ - private fields - ]

        /// <summary>
        /// Процессор BB кода
        /// </summary>
        public static readonly ThreadLocal<BBCodeParser> BbParser = new ThreadLocal<BBCodeParser>(
            () => new BBCodeParser(
                      ErrorMode.ErrorFree,
                      null,
                      new[]
                          {
                              new BBTag("b", "<b>", "</b>"),
                              new BBTag("i", "<span style=\"font-style:italic;\">", "</span>"),
                              new BBTag("u", "<span style=\"text-decoration:underline;\">", "</span>"),
                              new BBTag("s", "<s>", "</s>"),
                              new BBTag("sup", "<sup>", "</sup>"),
                              new BBTag("sub", "<sub>", "</sub>"),
                              new BBTag("youtube", "<iframe width='420' height='315' src='http://www.youtube.com/embed/${content}' frameborder='0' allowfullscreen></iframe>", "", false, true),
                              new BBTag("url", "<a href=\"${href}\">", "</a>", new BBAttribute("href", "", true), new BBAttribute("href", "href", true)),
                              new BBTag("list", "<ul>", "</ul>"),
                              new BBTag("nlist", "<ol>", "</ol>"),
                              new BBTag("*", "<li>", "</li>", true, false),
                              new BBTag("color", "<span style=\"color: ${color}\">", "</span>", new BBAttribute("color", ""), new BBAttribute("color", "color")),
                              new BBTag("size", "<span style=\"font-size:${size}em\">", "</a>", new BBAttribute("size", ""), new BBAttribute("size", "size")),
                              new BBTag("quote", "<blockquote>", "</blockquote>"),
                              new BBTag("img", "<img src=\"${content}\" />", "", false, true),
                              new BBTag("lightbox", "<a rel=\"lightbox\" href=\"${full}\"><img src=\"${content}\" /></a>", "", false, true, new BBAttribute("full", ""), new BBAttribute("full", "full")),
                              new BBTag("newscut", "<br />", "")
                          }
                      )
            );

        /// <summary>
        /// Регулярка для выделения ссылок в BBCode
        /// </summary>
        private static readonly Regex LinkToBbRegex = new Regex(
            @"(?<=\s|^)(?<url>(?:http|ftp|https)://(?:[\w+?\.\w+])+(?:[a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?)(?=\s|$)",
            RegexOptions.Compiled | RegexOptions.Multiline
            );

        /// <summary>
        /// Регулярка для выделения ссылок в BBCode
        /// </summary>
        private static readonly Regex NonTextRegex = new Regex(
            @"(<[^>]*>)|(\s+)",
            RegexOptions.Compiled | RegexOptions.Multiline
            );

        #endregion

        /// <summary>
        /// Выводит строчку соответствующую множественному числу
        /// </summary>
        /// <param name="html">Хэлпер</param>
        /// <param name="number">Число</param>
        /// <param name="one">Один...</param>
        /// <param name="four">Четыре...</param>
        /// <param name="seven">Семь...</param>
        /// <param name="zeroSpecial">Специальная строка для нуля. Если не задана, для нуля используется seven</param>
        /// <returns>Строка соответствующая числу</returns>
        public static string GetPlural(this HtmlHelper html, int number, string one, string four, string seven, string zeroSpecial = null)
        {
            if (number == 0 && !String.IsNullOrEmpty(zeroSpecial))
                return zeroSpecial.Replace("{0}", number.ToString(CultureInfo.InvariantCulture));

            number = Math.Abs(number);

            if (number%100 >= 11 && number%100 <= 19)
                return seven.Replace("{0}", number.ToString(CultureInfo.InvariantCulture));

            switch (number%10)
            {
                case 1:
                    return one.Replace("{0}", number.ToString(CultureInfo.InvariantCulture));
                case 2:
                case 3:
                case 4:
                    return four.Replace("{0}", number.ToString(CultureInfo.InvariantCulture));
                default:
                    return seven.Replace("{0}", number.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Проверяет, не является ли текст пустым
        /// </summary>
        /// <param name="text">Текст для проверки</param>
        /// <returns>Является ли текст пустым</returns>
        public static bool IsEmptyText(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                return true;

            //уберем теги
            text = NonTextRegex.Replace(text, "");

            //обработаем BB
            text = PrepareForDisplay(text);

            //если есть картинка, то не пустой
            if (text.Contains("<img"))
                return false;

            //уберем теги
            text = NonTextRegex.Replace(text, "");
            text = text.Replace("\r", "").Replace("\n", "");
            return String.IsNullOrWhiteSpace(text);
        }

        /// <summary>
        /// Обрезает тело новости
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CutBody(string text)
        {
            if (String.IsNullOrEmpty(text))
                return text;

            var cutPos = text.IndexOf("[newscut][/newscut]", StringComparison.OrdinalIgnoreCase);
            if (cutPos >= 0)
                return text.Substring(0, cutPos);
            
            return text;
        }

        /// <summary>
        /// Подготовка строки к отображению
        /// </summary>
        /// <param name="text">Текст для отображения</param>
        /// <returns>Результат</returns>
        public static string PrepareForDisplay(string text)
        {
            //проверим на Null
            if (String.IsNullOrEmpty(text))
                return text;

            //выделим бесхозные ссылки d BBCode
            text = processLinksToBbCode(text);

            //обработаем BBCode
            text = BbParser.Value.ToHtml(text);

            //заменим символы переноса строки
            return text
                .Replace("\r", "")
                .Replace("\n", "<br />");
        }

        /// <summary>
        /// Подготовка даты к отображению
        /// </summary>
        /// <param name="date">Дата для отображения</param>
        /// <param name="todayShort">Сегоднянюю дату коротко</param>
        /// <returns>Результат</returns>
        public static string PrepareForDisplay(DateTime date, bool todayShort = false)
        {
            if (date.Year == DateTime.Now.Year && date.DayOfYear == DateTime.Now.DayOfYear)
                return (todayShort ? "" : "сегодня в ") + date.ToShortTimeString();

            if (date.Year == DateTime.Now.Year && date.DayOfYear == DateTime.Now.DayOfYear - 1)
                return "вчера в " + date.ToShortTimeString();

            return date.ToLongDateString()
                   + " в "
                   + date.ToShortTimeString();
        }
        
        /// <summary>
        /// Выделяет ссылки в тексте в BBCode
        /// </summary>
        /// <param name="text">Текст для обработки</param>
        /// <returns>Текст с выделенными ссылками</returns>
        private static string processLinksToBbCode(string text)
        {
            return LinkToBbRegex.Replace(text, "[url=$0]$0[/url]");
        }

        public static string NormalizeBody(string text)
        {
            var regEx = new Regex(
                @"(?<url>(?:http|ftp|https)://(?:[\w+?\.\w+])+(?:[a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?)",
                RegexOptions.Compiled | RegexOptions.Multiline
                );
            var regExLink = new Regex(
                @"(?:url=)(?<url>(?:http|ftp|https)://(?:[\w+?\.\w+])+(?:[a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?)",
                RegexOptions.Compiled | RegexOptions.Multiline
                );
            var regExLinkCaption = new Regex(
                @"\]/Explorer#/",
                RegexOptions.Compiled | RegexOptions.Multiline
                );
            var regExTimeBB = new Regex(
                @"\[time=short\](?<timestamp>\d+)\[/time\]",
                RegexOptions.Compiled | RegexOptions.Multiline
                );
            var regExRu = new Regex("[а-яА-Я]");

            var regExSize = new Regex(
                @"\[size=(?<size>\d+)\]",
                RegexOptions.Compiled | RegexOptions.Multiline
                );

            var regYouTube = new Regex(
                @"\[youtube\]http://www\.youtube\.com/embed/(?<id>[a-zA-Z0-9\-]+)\[/youtube\]",
                RegexOptions.Compiled | RegexOptions.Multiline
                );

            //приведем ссылки к нормальному виду
            text = processLinksToBbCode(text);
            text = regEx.Replace(
                text,
                m =>
                    {
                        var value = m.Value;

                        value = value.Replace("http://f.ak-5.ru/?dir=", "/Explorer#/");

                        var decValueUtf = HttpUtility.UrlDecode(value, Encoding.UTF8);
                        if (regExRu.IsMatch(decValueUtf))
                            return decValueUtf;

                        var decValueWin = HttpUtility.UrlDecode(value, Encoding.GetEncoding("windows-1251"));
                        if (regExRu.IsMatch(decValueWin))
                            return decValueWin;

                        if (decValueUtf == decValueWin)
                            return value;

                        throw new ArgumentException();
                    }
                );

            text = regExLinkCaption.Replace(
                text,
                "]Файловый архив/"
                );

            text = regExTimeBB.Replace(
                text, 
                m=>
                    {
                        var date = new DateTime(1970, 1, 1);
                        date = date.AddSeconds(int.Parse(m.Groups["timestamp"].Value));
                        return PrepareForDisplay(date);
                    }
                );

            text = regExSize.Replace(
                text,
                m =>
                    {
                        var size = int.Parse(m.Groups["size"].Value);
                        if (size > 28)
                            return "[size=1.2]";
                        if(size>23)
                            return "[size=1.1]";
                        if(size>18)
                            return "[size=1]";
                        if (size > 13)
                            return "[size=0.9]";

                        return "[size=0.8]";
                    }
                );

            text = regYouTube.Replace(
                text,
                m => "[youtube]" + m.Groups["id"].Value + "[/youtube]"
                );
            

            return text;
        }
    }
}