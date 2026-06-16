using Entities;
using System.Threading.Tasks;

namespace Services
{
    public interface IKafkaProducerService
    {
        Task PublishOrderCreatedAsync(Order order);
    }
}
