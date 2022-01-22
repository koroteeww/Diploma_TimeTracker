using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// Описание файла
    /// </summary>
    [DataContract(Name = "fileInfo")]
    public class ExplorerFileModel
    {
        /// <summary>
        /// Имя файла
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Размер файла
        /// </summary>
        [DataMember(Name = "size")]
        public long Size { get; set; }

        /// <summary>
        /// Размер файла
        /// </summary>
        [DataMember(Name = "link")]
        public string Link { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        [DataMember(Name="date")]
        public string Date { get; set; }

        /// <summary>
        /// Является ли файл директорией
        /// </summary>
        [DataMember(Name = "isDirectory")]
        public bool IsDirectory { get; set; }

        /// <summary>
        /// Тип файла
        /// </summary>
        [DataMember(Name = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ExplorerFileType FileType { get; set; }
    }
}