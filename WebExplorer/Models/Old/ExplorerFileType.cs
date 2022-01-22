using System.Runtime.Serialization;

namespace WebExplorer.Models
{
    /// <summary>
    /// Тип файла
    /// </summary>
    [DataContract]
    public enum ExplorerFileType
    {
        [EnumMember(Value = "unknown")] Unknown = 0,
        [EnumMember(Value = "directory")] Directory = 1,
        [EnumMember(Value = "archive")] Archive = 2,
        [EnumMember(Value = "music")] Music = 3,
        [EnumMember(Value = "video")] Video = 4,
        [EnumMember(Value = "word")] Word = 5,
        [EnumMember(Value = "excel")] Excel = 6,
        [EnumMember(Value = "pdf")] Pdf = 7,
        [EnumMember(Value = "powerpoint")] PowerPoint = 8,
        [EnumMember(Value = "image")] Image = 9
    }
}