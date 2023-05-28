using ImageService.Models;

namespace Contracts
{
    public interface ImageHistory
    {
        public List<Image> Images { get; }
    }
}