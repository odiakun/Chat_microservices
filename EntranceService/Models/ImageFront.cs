namespace EntranceService.Models
{
    public record ImageFront
    {
        public String MessId { get; init; }
        public String User { get; init; }
        public String base64 { get; init; }
        public string mid { get; init; }
        public UInt64 Timestamp { get; init; }
    }
}