namespace ImageService.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using Services;
    using ImageService.Models;
    using System.IO;
    using Microsoft.AspNetCore.Http;

    public class ImageServiceConsumer : IConsumer<AddImage>, IConsumer<GetImageHistory>,
    IConsumer<DeleteImage>
    {
        private readonly ImagesService _imagesService;

        public ImageServiceConsumer(ImagesService imagesService)
        {
            _imagesService = imagesService;
        }

        public async Task Consume(ConsumeContext<AddImage> context)
        {
            string fileName = $"{context.Message.CommandId}_image.jpg";
            string imageDirectory = "Images"; // Directory name inside the container
            string filePath = Path.Combine(imageDirectory, fileName);
            File.WriteAllBytes(filePath, context.Message.rawImage.ImageBytes);
            
            
            string location = GlobalVariables.serviceAddress + fileName;
            string imageUrl = $"{location.Replace("\\", "/")}";

            Image image = new Image
            {
                MessId = context.Message.rawImage.MessId,
                User = context.Message.rawImage.User,
                url = imageUrl,
                mid = context.Message.rawImage.mid,
                Timestamp = context.Message.rawImage.Timestamp
            };

            await _imagesService.CreateAsync(image);

            await context.Publish<ImageAdded>(new {
                image = image
            });
        }
        public async Task Consume(ConsumeContext<GetImageHistory> context)
        {
            List<Image> images = await _imagesService.GetAsync();

            Console.WriteLine(images[0].url);

            await context.Publish<ImageHistory>(new {
                Images = images
            });  
        }
        public async Task Consume(ConsumeContext<DeleteImage> context)
        {
            Image image = await _imagesService.GetAsync(context.Message.MessId);
            Image modifiedImage = new Image
            {
                MessId = image.MessId,
                User = image.User,
                url = "",
                mid = image.mid,
                Timestamp = image.Timestamp
            };

            await _imagesService.UpdateAsync(context.Message.MessId, modifiedImage);
            await context.Publish<ImageDeleted>(new {
                MessId = context.Message.MessId
            });
        }
    }
}