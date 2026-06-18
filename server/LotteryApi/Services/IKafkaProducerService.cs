using LotteryApi.Dtos;

namespace LotteryApi.Services
{
    public interface IKafkaProducerService
    {
        Task PublishOrderEventAsync(string eventType, OrderDto order);
    }
}
