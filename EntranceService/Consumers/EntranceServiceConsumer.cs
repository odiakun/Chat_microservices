namespace EntranceService.Consumers {
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using EntranceService.Hubs;

    public class EntranceServiceConsumer :
    IConsumer<MessageAdded>, IConsumer<MessageDeleted>, IConsumer<UserFound>,
    IConsumer<UserNotFound>, IConsumer<UserAdded>, IConsumer<History>,
    IConsumer<ImageAdded>, IConsumer<ImageHistory>, IConsumer<ImageDeleted>{
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
        public async Task Consume(ConsumeContext<ImageAdded> context) {
            var chatHub = (IHubContext<ChatHub>)_serviceProvider.GetService(typeof(IHubContext<ChatHub>));
            await chatHub.Clients.All.SendAsync("ImageAdded", context.Message.image);
        }
        public async Task Consume(ConsumeContext<ImageHistory> context) {
            Console.WriteLine(context.Message.Images[0].url);
            var chatHub = (IHubContext<ChatHub>)_serviceProvider.GetService(typeof(IHubContext<ChatHub>));
            await chatHub.Clients.All.SendAsync("ImageHistory",context.Message.Images);
        }
        public async Task Consume(ConsumeContext<ImageDeleted> context) {
            var chatHub = (IHubContext<ChatHub>)_serviceProvider.GetService(typeof(IHubContext<ChatHub>));
            await chatHub.Clients.All.SendAsync("ImageDeleted", context.Message.MessId);
        }
    }
}