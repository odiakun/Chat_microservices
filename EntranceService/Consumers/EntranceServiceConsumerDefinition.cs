namespace EntranceService.Consumers {
    using MassTransit;

    public class EntranceServiceConsumerDefinition :
    ConsumerDefinition<EntranceServiceConsumer>{
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<EntranceServiceConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500,1000));
        }
    }
}