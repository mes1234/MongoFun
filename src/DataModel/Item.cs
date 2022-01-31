using MongoDB.Bson.Serialization.Attributes;

namespace DataModel
{
    [BsonIgnoreExtraElements]
    public class Item : ITimeStamped
    {
        public DateTime TimeStamp { get; set; }
        public ContentType ContentType { get; set; }
        public string? Name { get; set; }

        public byte[]? Data { get; set; }

    }

}