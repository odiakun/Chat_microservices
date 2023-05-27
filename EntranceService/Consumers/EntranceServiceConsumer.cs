namespace EntranceService.Consumers {
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using EntranceService.Hubs;

    public class EntranceServiceConsumer :
    IConsumer<MessageAdded>, IConsumer<MessageDeleted>, IConsumer<UserFound>,
    IConsumer<UserNotFound>, IConsumer<UserAdded>, IConsumer<History>{
        protected readonly IServiceProvider _serviceProvider;

        public EntranceServiceConsumer(IServiceProvider serviceProvider){
            _serviceProvider = serviceProvider;
        }
        public async Task Consume(ConsumeContext<MessageAdded> context) {
            var chatHub = (IHubContext<ChatHub>)_serviceProvider.GetService(typeof(IHubContext<ChatHub>));
            await chatHub.Clients.All.SendAsync("MessageReceived", context.Message.Message);
        }
        public async Task Consume(ConsumeContext<MessageDeleted> context) {
            var chatHub = (IHubContext<ChatHub>)_serviceProvider.GetService(typeof(IHubContext<ChatHub>));
            await chatHub.Clients.All.SendAsync("MessageDeleted", context.Message.MessId);
        }
        public async Task Consume(ConsumeContext<UserFound> context) {
            var chatHub = (IHubContext<ChatHub>)_serviceProvider.GetService(typeof(IHubContext<ChatHub>));
            await chatHub.Clients.All.SendAsync("UserFound", context.Message.Username);
        }
        public async Task Consume(ConsumeContext<UserNotFound> context) {
            var chatHub = (IHubContext<ChatHub>)_serviceProvider.GetService(typeof(IHubContext<ChatHub>));
            await chatHub.Clients.All.SendAsync("UserNotFound", context.Message.Username);
        }
        public async Task Consume(ConsumeContext<UserAdded> context) {
            var chatHub = (IHubContext<ChatHub>)_serviceProvider.GetService(typeof(IHubContext<ChatHub>));
            await chatHub.Clients.All.SendAsync("UserAdded", context.Message.Username);
        }
        public async Task Consume(ConsumeContext<History> context) {
            var chatHub = (IHubContext<ChatHub>)_serviceProvider.GetService(typeof(IHubContext<ChatHub>));
            await chatHub.Clients.All.SendAsync("History", context.Message.Messages);
        }
    }
}