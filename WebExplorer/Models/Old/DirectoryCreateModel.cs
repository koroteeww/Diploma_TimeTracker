using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// Класс описания информации создания папки
    /// </summary>
    [DataContract]
    public class DirectoryCreateModel
    {
        /// <summary>
        /// Путь к создаваемой папке
        /// </summary>
        [DataMember(Name="path")]
        public string Path { get; set; }

        /// <summary>
        /// Имя создаваемой папки
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}