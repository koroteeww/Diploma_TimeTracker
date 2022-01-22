using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebExplorer.Helpers;
using WebExplorer.Models;

namespace WebExplorer.Controllers
{
    public class FilesController : ApiController
    {
        /// <summary>
        /// Список файлов
        /// </summary>
        public IEnumerable<ExplorerFileModel> Get(string path = "")
        {
            //получим ссылку на информацию о директории
            var info = WePathHelper.GetDirectoryInfoIfAllowed(path);
            //вернем список файлов
            return info
                .GetDirectories()
                .Where(d => !WePathHelper.Current.HideNames.Contains(d.Name))
                .Select(d => d.ToExplorerFileModel())
                .Union(
                    info
                        .GetFiles()
                        .Where(f => !WePathHelper.Current.HideNames.Contains(f.Name))
                        .Where(f => !WePathHelper.Current.HideExtensions.Contains(f.Extension))
                        .Select(f => f.ToExplorerFileModel())
                );
        }

        /// <summary>
        /// Отправка файла
        /// </summary>
        [Authorize]
        public async Task<HttpResponseMessage> Post()
        {
            try
            {
                //нам подходит только multipart/form-data
                if (!Request.Content.IsMimeMultipartContent())
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

                //провайдер для чтения информации из запроса
                var provider = new MultipartFormDataStreamProvider(WePathHelper.Current.UploadTempPath);
                await Request.Content.ReadAsMultipartAsync(provider);

                try
                {
                    //получим ссылку на директорию
                    var localPath = provider.FormData["path"];
                    var info = WePathHelper.GetDirectoryInfoIfAllowed(localPath);

                    //сохраним файлы
                    Parallel.ForEach(
                        provider.FileData,
                        file =>
                        {
                            //получим исходное имя файла
                            var fileName = (file.Headers.ContentDisposition.FileName ?? String.Empty)
                                .Trim(new[] { ' ', '"' });
                            if (String.IsNullOrEmpty(fileName)) return;

                            //сохраняем файлы
                            File.Move(
                                file.LocalFileName,
                                Path.Combine(info.FullName, fileName)
                                );
                        }
                        );

                    return new HttpResponseMessage(HttpStatusCode.Created)
                        {
                            Content = new StringContent(true.ToString())
                        };
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Ошибка сохранения файлов.", ex);
                }
                finally
                {
                    Parallel.ForEach(
                        provider.FileData
                            .AsParallel()
                            .Select(f => f.LocalFileName)
                            .Where(File.Exists),
                        File.Delete
                        );
                }
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.Created)
                    {
                        Content = new StringContent(false.ToString())
                    };
            }
            
        }
    }
}

