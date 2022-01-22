using WebExplorer.Auth;
using WebExplorer.Entity;
using WebExplorer.Helpers;

namespace WebExplorer.Models
{
    public static class NewsCommentModelExtensions
    {
        /// <summary>
        /// Создает модель комментария на основе комментария из БД
        /// </summary>
        /// <param name="comment">Комментарий из БД</param>
        /// <returns>Модель комментария</returns>
        public static NewsCommentModel ToNewsCommentModel(this NewsComments comment)
        {
            var user = WeMembership.GetCurrentUser();
            return new NewsCommentModel
                {
                    Id = comment.Id,
                    Author = comment.Author.Login,
                    AuthorId = comment.Author.Id,
                    NewsId = comment.News.Id,
                    Date = TextHelper.PrepareForDisplay(comment.CreationDate),
                    Body = TextHelper.PrepareForDisplay(comment.Body),
                    CanEdit = (comment.Author.Id==user.Id || user.IsAdmin)
                };
        }
    }
}