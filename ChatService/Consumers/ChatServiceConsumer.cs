namespace ChatService.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using Services;
    using ChatService.Models;

    public class ChatServiceConsumer : IConsumer<AddMessage>, IConsumer<DeleteMessage>
    {
        private readonly MessageService _messagesService;

        public ChatServiceConsumer(MessageService messageService) =>
        _messagesService = messageService;

        public async Task Consume(ConsumeContext<AddMessage> context)
        {
            Console.WriteLine($"Chat.hpds received AddMessage message, message id: {context.Message.Message.mid}");
            Message message = new Message 
            {
                MessId = context.Message.Message.MessId,
                mid = context.Message.Message.mid,
                User = context.Message.Message.User,
                Text = context.Message.Message.Text,
                Timestamp = context.Message.Message.Timestamp
            };
            await _messagesService.CreateAsync(message);
            await context.Publish<MessageAdded>(new
            {
                Message = context.Message.Message
            });
            Console.WriteLine($"Chat.hpds sending message, message id: {context.Message.Message.mid}");
        }
        public async Task Consume(ConsumeContext<DeleteMessage> context)
        {
            Message message = await _messagesService.GetAsync(context.Message.MessId);
            Message modifiedMessage = new Message
            {
                MessId = message.MessId,
                mid = message.mid,
                User = message.User,
                Text = "Message deleted",
                Timestamp = message.Timestamp
            };
            await _messagesService.UpdateAsync(context.Message.MessId, modifiedMessage);
            await context.Publish<MessageDeleted>(new
            {
                MessId = context.Message.MessId
            });
        }
    }
}