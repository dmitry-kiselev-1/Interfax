using System.Runtime.Serialization;

namespace Interfax.FileCollector.Web.Service.DataContracts
{
    [DataContract]
    public class FileStorage
    {
        [DataMember] public System.Guid Id { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Path { get; set; }
        [DataMember] public string Type { get; set; }
        [DataMember] public long? Size { get; set; }
        [DataMember] public System.DateTimeOffset CreateDate { get; set; }
        [DataMember] public System.DateTimeOffset LoadDate { get; set; }
        [DataMember] public byte[] Data { get; set; }
    }
}