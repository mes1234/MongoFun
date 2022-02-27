using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel
{
    [BsonIgnoreExtraElements]
    public class Item : ITimeStamped
    {
        [NotMapped]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? MongoId { get; set; }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime TimeStamp { get; set; }
        public ContentType? ContentType { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public byte[]? Data { get; set; }

    }

}