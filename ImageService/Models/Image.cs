using MongoDB.Bson.Serialization.Attributes;

namespace ImageService.Models
{
    [BsonNoId]
[BsonIgnoreExtraElements]
    public record Image
    {
        public String MessId { get; init; }
        public String User { get; init; }
        public String url { get; init; }
        public string mid { get; init; }
        public UInt64 Timestamp { get; init; }
    }
}