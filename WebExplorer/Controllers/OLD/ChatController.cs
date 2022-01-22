using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using WebExplorer.Auth;
using WebExplorer.Entity;
using WebExplorer.Helpers;
using WebExplorer.Models;

namespace WebExplorer.Controllers
{
    public class ChatController : ApiController
    {
        /// <summary>
        /// Коллекция сообщений в чате
        /// </summary>
        private static readonly ConcurrentDictionary<int, ChatMessageModel> Messages;

        /// <summary>
        /// Статический инициализатор чата
        /// </summary>
        static ChatController()
        {
            /*
            //инициализируем коллекцию сообщений в чате
            Messages = new ConcurrentDictionary<int, ChatMessageModel>();
            using (var db = new WeContext())
            {
                foreach (var message in db.ChatMessages
                    .Include(m => m.Author)
                    .OrderByDescending(m => m.CreationDate)
                    .Take(30)
                    .OrderBy(m => m.CreationDate))
                {
                    Messages.GetOrAdd(message.Id, new ChatMessageModel
                        {
                            Id = message.Id,
                            Date = TextHelper.PrepareForDisplay(message.CreationDate, true),
                            Author = message.Author.Login,
                            Body = message.Text,
                            AuthorId = message.Author.Id
                        });
                }
            }
             */
        }

        /// <summary>
        /// Добавление сообщения
        /// api/chat/post
        /// </summary>
        /// <param name="message">Сообщение</param>
        [Authorize]
        [HttpPost]
        public System.Threading.Tasks.Task Post(ChatMessageModel message)
        {
            /*
            //асинхронно сохраняем сообщение
            return System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    //проверим, что там на пришло
                    if (TextHelper.IsEmptyText(message.Body))
                        return;

                    //идентификатор сообщения
                    int newMessageId;

                    var currentUser = WeMembership.GetUser(User.Identity.Name);
                    
                    //в базу
                    using (var db = new WeContext())
                    {
                        var authorId = currentUser.Id;
                        var newMessage = db.ChatMessages.Create();
                        newMessage.Text = message.Body;
                        newMessage.Author = db.WeUsers.FirstOrDefault(u => u.Id == authorId);
                        newMessage.CreationDate = DateTime.Now;
                        db.ChatMessages.Add(newMessage);

                        db.SaveChanges();

                        newMessageId = newMessage.Id;
                    }

                    //в чатик
                    message.Id = newMessageId;
                    message.Date = TextHelper.PrepareForDisplay(DateTime.Now, true);
                    message.Author = currentUser.Login;
                    message.AuthorId = currentUser.Id;
                    Messages.GetOrAdd(newMessageId, message);

                    //запускаем все потоки, ждущие обработки
                    lock (Messages)
                    {
                        Monitor.PulseAll(Messages);
                    }
                });
             * */
            return null;
        }

        /// <summary>
        /// Список сообщений чата
        /// GET api/chat
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public Task<IEnumerable<ChatMessageModel>> Get(int? id)
        {
            return Task<IEnumerable<ChatMessageModel>>.Factory.StartNew(() =>
                {
                    //если нет новых сообщений, подождем
                    var gotNewMessages = true;
                    if (!Messages.Any(m => !id.HasValue || m.Key > id.Value))
                    {
                        lock (Messages)
                        {
                            gotNewMessages = Monitor.Wait(Messages, new TimeSpan(0, 0, 30));
                        }
                    }

                    //если не получили сообщений, вернем пустой массив
                    if (!gotNewMessages)
                        return new ChatMessageModel[0];

                    //если новые сообщения есть, выберем их и вернем
                    return Messages
                        .Where(m => !id.HasValue || m.Key > id.Value)
                        .OrderBy(m => m.Value.Id)
                        .Select(m => m.Value);
                });
        }
    }
}
