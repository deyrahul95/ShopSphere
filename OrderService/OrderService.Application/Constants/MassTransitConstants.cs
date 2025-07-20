namespace OrderService.Application.Constants;

public class MassTransitConstants
{
    public const int MaxRetryCount = 3;
    public const int RetryTimeSpanInSecond = 5;
    public const string RabbitMqHost = "rabbitmq";
    public const string RabbitMqDefaultUser = "guest";
    public const string RabbitMqDefaultPassword = "guest";
    public const string NotificationQueueName = "notification-queue";
    public const string DeadLetterExchangeName = "notification-dead-letter-exchange";
}
