using System.Text;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebExplorer.Helpers;

namespace WebExplorer.Controllers
{
    public class UploadImageController : ApiController
    {
        /// <summary>
        /// Загрузка изображения
        /// POST api/uploadimage
        /// </summary>
        [Authorize]
        public async Task<HttpResponseMessage> Post()
        {
            //нам подходит только multipart/form-data
            if (!Request.Content.IsMimeMultipartContent())
                return prepareWysiBbErrorResponse("Используйте multipart\\form-data для отправки");

            //провайдер для чтения информации из запроса
            var provider = new MultipartFormDataStreamProvider(WePathHelper.Current.UploadTempPath);
            await Request.Content.ReadAsMultipartAsync(provider);

            try
            {
                //загружаем только первый файл
                var fileData = provider.FileData.FirstOrDefault();

                //убедимся что файл прикреплен
                if (fileData == null || fileData.Headers.ContentDisposition.FileName == null)
                    throw new ArgumentException("Не прикреплен файл");

                //проверим расширение
                var fileName = (fileData.Headers.ContentDisposition.FileName ?? String.Empty)
                    .Trim(new[] {' ', '"'});

                var ext = (Path.GetExtension(fileName) ?? String.Empty).ToLower();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif")
                    throw new ArgumentException("Тип файла не разрешен к загрузке");

                //получим параметры
                int maxWidth;
                int maxHeight;
                if (!int.TryParse(provider.FormData["maxwidth"], out maxWidth))
                    maxWidth = 600;
                if (!int.TryParse(provider.FormData["maxheight"], out maxHeight))
                    maxHeight = 600;
                bool isIframe = !String.IsNullOrEmpty(provider.FormData["iframe"]);
                string idArea = provider.FormData["idarea"];

                //скопируем файл
                var localFileName = Guid.NewGuid().ToString() + ext;
                var localFilePath = Path.Combine(WePathHelper.Current.ImageUploadServerPath, localFileName);
                

                //изменим картинку
                using (var srcImage = new Bitmap(fileData.LocalFileName))
                {
                    using (var resizedImage = getResizedImage(srcImage, maxWidth, maxHeight))
                    {
                        if (resizedImage != null)
                            resizedImage.Save(localFilePath);
                        else
                            File.Move(fileData.LocalFileName, localFilePath);
                    }
                    using (var resizedImage = getResizedImage(srcImage, 250, 250))
                    {
                        if (resizedImage != null)
                            resizedImage.Save(localFilePath + ".small" + ext);
                    }
                }

                //вернем ответ
                var fileVirtualPath = WePathHelper.Current.ImageUploadVirtualPath + "/" + localFileName;
                return prepareWysiSuccesBbResponse(
                    fileVirtualPath,
                    fileVirtualPath + ".small" + ext,
                    isIframe,
                    idArea
                    );
            }
            catch (Exception ex)
            {
                return prepareWysiBbErrorResponse(ex.Message);
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

        /// <summary>
        /// Сжимает картинку, если это необходимо
        /// </summary>
        /// <param name="srcImage">Исходное изображение</param>
        /// <param name="maxWidth">Максимальная ширина</param>
        /// <param name="maxHeight">Максимальная высота</param>
        /// <returns>Сжатое изображение или null если сжатие не требуется</returns>
        private Bitmap getResizedImage(Bitmap srcImage, int maxWidth, int maxHeight)
        {
            if (srcImage.Width <= maxWidth && srcImage.Height <= maxHeight)
                return null;

            var factor = Math.Max(srcImage.Width/(float) maxWidth, srcImage.Height/(float) maxHeight);
            var newWidth = (int) Math.Round(srcImage.Width/factor);
            var newHeight = (int) Math.Round(srcImage.Height/factor);

            var newImage = new Bitmap(newWidth, newHeight);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));
            }

            return newImage;
        }

        /// <summary>
        /// Подготавливает результат для отправки редактору. В случае если это iframe, возвращаем скрипт, если нет, json
        /// </summary>
        /// <param name="response">Текстовый ответ</param>
        /// <returns>результат для отправки редактору</returns>
        private HttpResponseMessage prepareWysiBbErrorResponse(string response)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(new
                            {
                                status = 0,
                                msg = response
                            }),
                        Encoding.UTF8,
                        "text/javascript")
                };
        }

        /// <summary>
        /// Подготавливает результат для отправки редактору. В случае если это iframe, возвращаем скрипт, если нет, json
        /// </summary>
        /// <param name="link">URL картинки</param>
        /// <param name="thumb">URL превьюшки</param>
        /// <param name="isIframe">Возвращать результат в iframe виде</param>
        /// <param name="idArea">ID для результата в iframe форме</param>
        /// <returns>результат для отправки редактору</returns>
        private HttpResponseMessage prepareWysiSuccesBbResponse(string link, string thumb, bool isIframe, string idArea)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        isIframe
                            ? String.Format(
                                "<html><body>OK<script>window.parent.$(\"#{0}\").insertImage(\"{1}\",\"{2}\").closeModal().updateUI();</script></body></html>",
                                idArea,
                                link,
                                thumb
                                  )
                            : JsonConvert.SerializeObject(new
                                {
                                    status = 1,
                                    msg = "OK",
                                    image_link = link,
                                    thumb_link = thumb
                                }),
                        Encoding.UTF8,
                        isIframe ? "text/html" : "text/javascript"
                        )
                };
        }
    }
}
