using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// Модель которая может содержать ошибки
    /// </summary>
    [DataContract]
    public abstract class ErrorsModel
    {
        /// <summary>
        /// Новый экземпляр модели с ошибкой
        /// </summary>
        /// <param name="error">Текст ошибки</param>
        /// <returns>Модель с ошибкой</returns>
        public static T CreateWithError<T>(string error) where T : ErrorsModel, new()
        {
            var result =  new T();
            result.AddError(error);
            return result;
        }

        /// <summary>
        /// Ошибки
        /// </summary>
        private readonly List<string> _errors = new List<string>();

        /// <summary>
        /// Ошибки
        /// </summary>
        [DataMember(Name = "errors")]
        public IEnumerable<string> Errors
        {
            get { return _errors; }
        }

        /// <summary>
        /// Добавляет ошибку в коллекицию, если такой там еще нет
        /// </summary>
        /// <param name="error"></param>
        public void AddError(string error)
        {
            if (!_errors.Contains(error, StringComparer.OrdinalIgnoreCase))
                _errors.Add(error);
        }
    }
}