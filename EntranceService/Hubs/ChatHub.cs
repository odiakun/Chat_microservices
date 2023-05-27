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
    }
}