namespace ImageService.Consumers
{
    using MassTransit;

    public class ImageServiceConsumerDefinition : ConsumerDefinition<ImageServiceConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ImageServiceConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500,1000));
        }
    }
}