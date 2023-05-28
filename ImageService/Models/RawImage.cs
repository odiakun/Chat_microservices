namespace ImageService.Models
{
    public record RawImage
    {
        public String MessId { get; init; }
        public String User { get; init; }
        public byte[] ImageBytes { get; init;}
        public string mid { get; init; }
        public UInt64 Timestamp { get; init; }
    }
}