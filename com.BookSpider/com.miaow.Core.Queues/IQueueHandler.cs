namespace com.miaow.Core.Queues
{
    public interface IQueueHandler
    {
        string HostName { get; set; }
        string QueueName { get; set; }
    }
}