using OrderService.Domain.Entities;

namespace OrderService.Domain.Repositories;

public interface IOrderRepository
{
    Task<Order> CreateOrder(Order order, CancellationToken cancellationToken = default);
    Task<Order?> GetOrder(Guid orderId, CancellationToken cancellationToken = default);
    Task<Order> UpdateOrder(Order order, CancellationToken cancellationToken);    
}
