namespace EntranceService.Consumers {
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using EntranceService.Hubs;

    public class EntranceServiceConsumer :
    IConsumer<MessageAdded>, IConsumer<MessageDeleted> {
        protected readonly IServiceProvider _serviceProvider;

        public EntranceServiceConsumer(IServiceProvider serviceProvider){
            _serviceProvider = serviceProvider;
        }
        public async Task Consume(ConsumeContext<MessageAdded> context) {
            var chatHub = (IHubContext<ChatHub>)_serviceProvider.GetService(typeof(IHubContext<ChatHub>));
            Console.WriteLine($"Entrance service received message from rabbitmq, mess id: {context.Message.Message.mid}");
            await chatHub.Clients.All.SendAsync("MessageReceived", context.Message.Message);
        }
        public async Task Consume(ConsumeContext<MessageDeleted> context) {
            var chatHub = (IHubContext<ChatHub>)_serviceProvider.GetService(typeof(IHubContext<ChatHub>));
            await chatHub.Clients.All.SendAsync("MessageDeleted", context.Message.MessId);
            Console.WriteLine($"Entrance relaying message about message deletion, mid: {context.Message.MessId}");
        }
    }
}