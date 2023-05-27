namespace LoginService.Consumers{
    using MassTransit;

    public class LoginServiceConsumerDefinition : 
    ConsumerDefinition<LoginServiceConsumer>{
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<LoginServiceConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500,1000));
        }
    }
}