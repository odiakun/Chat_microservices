namespace Contracts
{
    public interface DeleteImage
    {
        public Guid CommandId { get; }
        public DateTime Timestamp { get; }
        public String MessId { get; }
    }
}