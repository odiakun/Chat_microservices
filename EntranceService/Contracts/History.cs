using EntranceService.Models;

namespace Contracts
{
    public interface History
    {
        public List<Message> Messages { get; }
    }
}