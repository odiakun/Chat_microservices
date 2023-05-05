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
            Console.WriteLine($"Message received at entrance.hpds hub, message id: {message.mid}");
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
    }
}