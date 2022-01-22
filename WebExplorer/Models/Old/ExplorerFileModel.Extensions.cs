using System;
using System.IO;
using WebExplorer.Helpers;

namespace WebExplorer.Models
{
    public static class ExplorerFileModelExtensions
    {
        /// <summary>
        /// Сохрарение базовой информации о элементе файловой системы
        /// </summary>
        private static ExplorerFileModel toExplorerFileModel (this FileSystemInfo info)
        {
            return new ExplorerFileModel
                {
                    Name = info.Name,
                    Link = info.FullName
                        .Substring(WePathHelper.Current.RootPath.Length)
                        .Replace(Path.DirectorySeparatorChar, '/')
                        .Replace(Path.AltDirectorySeparatorChar, '/')
                        .Trim(new[] {'/'})
                };
        }

        /// <summary>
        /// Создание информации о файле
        /// </summary>
        public static ExplorerFileModel ToExplorerFileModel (this FileInfo info)
        {
            var result = info.toExplorerFileModel();

            result.Size = info.Length;
            result.Date = info.CreationTime.ToLongDateString();
            result.IsDirectory = false;
            result.Link = WePathHelper.Current.LinkTemplate + result.Link;
            #region [ - определение типа файла - ]
            switch (info.Extension.ToLower())
            {
                case ".zip":
                case ".rar":
                case ".tar":
                case ".gz":
                case ".7z":
                    result.FileType = ExplorerFileType.Archive;
                    break;
                case ".doc":
                case ".docx":
                case ".odt":
                case ".txt":
                case ".rtf":
                    result.FileType = ExplorerFileType.Word;
                    break;
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".png":
                case ".tiff":
                case ".bmp":
                    result.FileType = ExplorerFileType.Image;
                    break;
                case ".pdf":
                    result.FileType = ExplorerFileType.Pdf;
                    break;
                case ".ppt":
                case ".pptx":
                    result.FileType = ExplorerFileType.PowerPoint;
                    break;
                case ".xls":
                case ".xlsx":
                    result.FileType = ExplorerFileType.Excel;
                    break;
                case ".mp3":
                case ".ogg":
                case ".aac":
                    result.FileType = ExplorerFileType.Music;
                    break;
                case ".avi":
                case ".mpg":
                case ".mpeg":
                case ".mkv":
                case ".mp4":
                    result.FileType = ExplorerFileType.Video;
                    break;
                default:
                    result.FileType = ExplorerFileType.Unknown;
                    break;
            }
            #endregion

            return result;
        }

        /// <summary>
        /// Создание информации о файле
        /// </summary>
        public static ExplorerFileModel ToExplorerFileModel (this DirectoryInfo info)
        {
            var result = info.toExplorerFileModel();

            result.Size = 0;
            result.Date = String.Empty;
            result.IsDirectory = true;
            result.FileType = ExplorerFileType.Directory;

            return result;
        }
    }
}