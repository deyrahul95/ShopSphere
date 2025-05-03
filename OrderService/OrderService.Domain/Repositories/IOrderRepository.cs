using OrderService.Domain.Entities;

namespace OrderService.Domain.Repositories;

public interface IOrderRepository
{
    Task<Order> CreateOrder(Order order, CancellationToken cancellationToken = default);
    Task<Order?> GetOrder(Guid userId, Guid orderId, CancellationToken cancellationToken = default);
    Task<bool> UpdateOrder(Order order, CancellationToken cancellationToken = default);    
}
