using ChatService.Models;

namespace Contracts
{
    public interface MessageAdded
    {
        public Message Message { get; }
    }
}