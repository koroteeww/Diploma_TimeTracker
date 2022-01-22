using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// Информация о файле
    /// </summary>
    [DataContract]
    public class UserModel
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        [DataMember(Name = "login")]
        public string Login { get; set; }

    }
}