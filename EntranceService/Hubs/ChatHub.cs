using MassTransit;
using Microsoft.AspNetCore.SignalR;
using EntranceService.Models;
using Contracts;

namespace EntranceService.Hubs {
    public class ChatHub : Hub {
        readonly IPublishEndpoint _publishEndpoint;

        public ChatHub(IPublishEndpoint publishEndpoint){
            _publishEndpoint = publishEndpoint;
        }

        public async Task SendMessage(Message message){
            await _publishEndpoint.Publish<AddMessage>(new {
                CommandId = NewId.NextGuid(),
                Timestamp = DateTime.Now,
                Message = message
            });
        }
        public async Task DeleteMessage(String messageId){
            await _publishEndpoint.Publish<DeleteMessage>(new {
                CommandId = NewId.NextGuid(),
                Timestamp = DateTime.Now,
                MessId = messageId
            });
        }
        public async Task GetUser(String username){
            await _publishEndpoint.Publish<GetUser>(new {
                CommandId = NewId.NextGuid(),
                Timestamp = DateTime.Now,
                Username = username
            });
        }
        public async Task CreateUser(User user){
            await _publishEndpoint.Publish<AddUser>(new {
                CommandId = NewId.NextGuid(),
                Timestamp = DateTime.Now,
                UserDTO = user
            });
        }
        public async Task DeleteUser(string username){
            await _publishEndpoint.Publish<DeleteUser>(new {
                CommandID = NewId.NextGuid(),
                Timestamp = DateTime.Now,
                Username = username
            });
        }
        public async Task GetHistory(){
            await _publishEndpoint.Publish<GetHistory>(new {
                CommandID = NewId.NextGuid(),
                Timestamp = DateTime.Now
            });
        }
        public async Task Typing(String name) => await Clients.All.SendAsync("SomeoneTyping", name);
        public async Task AddImage(ImageFront image)
        {
            byte[] imageData = Convert.FromBase64String(image.base64);
            RawImage rawImage = new RawImage
            {
                MessId = image.MessId,
                User = image.User,
                ImageBytes = imageData,
                mid = image.mid,
                Timestamp = image.Timestamp
            };

            await _publishEndpoint.Publish<AddImage>(new {
                CommandId = NewId.NextGuid(),
                Timestamp = DateTime.Now,
                rawImage = rawImage
            });
        }
        public async Task GetImageHistory()
        {
            await _publishEndpoint.Publish<GetImageHistory>(new {
                CommandID = NewId.NextGuid(),
                Timestamp = DateTime.Now
            });
        }
        public async Task DeleteImage(string messageId)
        {
            await _publishEndpoint.Publish<DeleteImage>(new {
                CommandId = NewId.NextGuid(),
                Timestamp = DateTime.Now,
                MessId = messageId
            });
        }
    }
}