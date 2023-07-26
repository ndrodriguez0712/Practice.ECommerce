namespace ServiceBusProvider.Interfaces
{
    public interface IServiceBusQueue
    {
        Task SendMessageAsync(string queueName, string serviceBusMessage);
    }
}
