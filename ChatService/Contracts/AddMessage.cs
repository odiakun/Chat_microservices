using ChatService.Models;

namespace Contracts
{
    public interface AddMessage
    {
        public Guid CommandId { get; }
        public DateTime Timestamp { get; }
        public Message Message { get; }
    }
}