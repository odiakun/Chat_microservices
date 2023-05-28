using EntranceService.Models;

namespace Contracts
{
    public interface AddImage
    {
        public Guid CommandId { get; }
        public DateTime Timestamp { get; }
        public RawImage rawImage { get; }
    }
}