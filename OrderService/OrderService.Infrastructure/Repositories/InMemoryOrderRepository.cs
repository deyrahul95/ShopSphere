using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;
using OrderService.Infrastructure.DB;

namespace OrderService.Infrastructure.Repositories;

public class InMemoryOrderRepository(InMemoryDB db) : IOrderRepository
{
    public async Task<Order> CreateOrder(Order order, CancellationToken cancellationToken = default)
    {
        if (db.Orders.TryGetValue(order.UserId, out List<Order>? orders))
        {
            orders.Add(order);
        }
        else
        {
            db.Orders[order.UserId] = [order];
        }

        return await Task.FromResult(order);
    }

    public async Task<Order?> GetOrder(Guid userId, Guid orderId, CancellationToken cancellationToken = default)
    {
        db.Orders.TryGetValue(userId, out var orders);

        if (orders is null || orders.Count == 0)
        {
            return null;
        }

        var order = orders.FirstOrDefault(o => o.Id == orderId);

        return await Task.FromResult(order);
    }

    public async Task<bool> UpdateOrder(Order order, CancellationToken cancellationToken = default)
    {
        db.Orders.TryGetValue(order.UserId, out var orders);

        if (orders is null || orders.Count == 0)
        {
            return await Task.FromResult(false);
        }

        var existingOrder = orders.FirstOrDefault(o => o.Id == order.Id);

        if (existingOrder is null)
        {
            return await Task.FromResult(false);
        }

        orders.Remove(existingOrder);
        orders.Add(order);

        return await Task.FromResult(true);
    }
}
