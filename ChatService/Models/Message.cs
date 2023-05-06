using MongoDB.Bson.Serialization.Attributes;

namespace ChatService.Models
{
    [BsonNoId]
[BsonIgnoreExtraElements]
    public record Message
    {
        public String MessId { get; init; }
        public String User { get; init; }
        public String Text { get; init; }
        // [BsonId]
        public string mid { get; init; }
        public UInt64 Timestamp { get; init; }
    }
}