using System;
using System.Text;
using System.Net.Mail;
using WebExplorer.Entity;
namespace WebExplorer.Helpers
{
    /// <summary>
    /// Отправка писем на e-mail
    /// </summary>
    public static class EmailHelper
    {
        #region private members

        /// <summary>
        /// пароль к ящику на ak-5.ru для отправки писем
        /// не знаю куда его пихнуть, может хранить в БД или в зашифрованном виде?
        /// и расшифровывать при необходимости
        /// </summary>
        private static string PasswordForSending = System.Configuration.ConfigurationManager.AppSettings["smtpPassword"];

        /// <summary>
        /// smtp сервер
        /// </summary>
        private static string HostForSending = System.Configuration.ConfigurationManager.AppSettings["smtpHost"];

        /// <summary>
        /// ящик для отправки писем пользователям
        /// </summary>
        private static string EmailForSending = System.Configuration.ConfigurationManager.AppSettings["smtpEmail"];

        
        #endregion
        #region public members
        /// <summary>
        /// Отправляет е-мейл на заданный адрес с заданной темой и телом
        /// </summary>
        /// <param name="emailto">мыло куда отправим</param>
        /// <param name="subject">тема письма</param>
        /// <param name="body">тело письма</param>
        public static void SendEmail(string emailto,string subject,string body)
        {
            //создаем новое сообщение
            var message = new MailMessage(EmailForSending, emailto)
                {
                    Subject = subject, 
                    Body = body, 
                    BodyEncoding = Encoding.UTF8,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                    IsBodyHtml = false,
                    Priority = MailPriority.Normal, 
                    SubjectEncoding = Encoding.UTF8
                };
            message.ReplyToList.Add(new MailAddress(EmailForSending));

            //подключаемся к smtp серверу для его отправки (стандартно порт 25)
            var client = new SmtpClient(HostForSending)
                {
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(EmailForSending, PasswordForSending), 
                    Host = HostForSending, 
                    Timeout = 100000
                };

            //отправляем синхронно ибо иначе ошибка "В данный момент невозможно запустить асинхронную операцию"
            client.Send(message);
        }

        #endregion
    }



}
