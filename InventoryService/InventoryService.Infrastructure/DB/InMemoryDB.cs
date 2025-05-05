using InventoryService.Domain.Entities;

namespace InventoryService.Infrastructure.DB;

public class InMemoryDB
{
    public readonly List<ProductStock> ProductStocks = [
        new ProductStock
        {
            ProductId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            AvailableQuantity = 50
        },
        new ProductStock
        {
            ProductId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            AvailableQuantity = 40
        },
        new ProductStock
        {
            ProductId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            AvailableQuantity = 0
        },
        new ProductStock
        {
            ProductId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            AvailableQuantity = 25
        },
        new ProductStock
        {
            ProductId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            AvailableQuantity = 0
        },
        new ProductStock
        {
            ProductId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            AvailableQuantity = 60
        },
        new ProductStock
        {
            ProductId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
            AvailableQuantity = 30
        },
        new ProductStock
        {
            ProductId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
            AvailableQuantity = 0
        },
        new ProductStock
        {
            ProductId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            AvailableQuantity = 0
        },
        new ProductStock
        {
            ProductId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            AvailableQuantity = 0
        },
        new ProductStock
        {
            ProductId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            AvailableQuantity = 90
        },
        new ProductStock
        {
            ProductId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            AvailableQuantity = 75
        },
        new ProductStock
        {
            ProductId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
            AvailableQuantity = 0
        },
        new ProductStock
        {
            ProductId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
            AvailableQuantity = 10
        },
        new ProductStock
        {
            ProductId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
            AvailableQuantity = 35
        },
        new ProductStock
        {
            ProductId = Guid.Parse("10101010-1010-1010-1010-101010101010"),
            AvailableQuantity = 30
        },
        new ProductStock
        {
            ProductId = Guid.Parse("20202020-2020-2020-2020-202020202020"),
            AvailableQuantity = 50
        },
        new ProductStock
        {
            ProductId = Guid.Parse("30303030-3030-3030-3030-303030303030"),
            AvailableQuantity = 40
        },
        new ProductStock
        {
            ProductId = Guid.Parse("40404040-4040-4040-4040-404040404040"),
            AvailableQuantity = 20
        },
        new ProductStock
        {
            ProductId = Guid.Parse("50505050-5050-5050-5050-505050505050"),
            AvailableQuantity = 10
        },
        new ProductStock
        {
            ProductId = Guid.Parse("60606060-6060-6060-6060-606060606060"),
            AvailableQuantity = 50
        },
        new ProductStock
        {
            ProductId = Guid.Parse("70707070-7070-7070-7070-707070707070"),
            AvailableQuantity = 60
        },
        new ProductStock
        {
            ProductId = Guid.Parse("80808080-8080-8080-8080-808080808080"),
            AvailableQuantity = 45
        },
        new ProductStock
        {
            ProductId = Guid.Parse("90909090-9090-9090-9090-909090909090"),
            AvailableQuantity = 25
        },
        new ProductStock
        {
            ProductId = Guid.Parse("10101010-1010-1010-1010-101010101010"),
            AvailableQuantity = 20
        },
        new ProductStock
        {
            ProductId = Guid.Parse("20202020-2020-2020-2020-202020202020"),
            AvailableQuantity = 35
        },
        new ProductStock
        {
            ProductId = Guid.Parse("30303030-3030-3030-3030-303030303030"),
            AvailableQuantity = 50
        },
        new ProductStock
        {
            ProductId = Guid.Parse("40404040-4040-4040-4040-404040404040"),
            AvailableQuantity = 55
        },
        new ProductStock
        {
            ProductId = Guid.Parse("50505050-5050-5050-5050-505050505050"),
            AvailableQuantity = 30
        },
        new ProductStock
        {
            ProductId = Guid.Parse("60606060-6060-6060-6060-606060606060"),
            AvailableQuantity = 10
        },
        new ProductStock
        {
            ProductId = Guid.Parse("70707070-7070-7070-7070-707070707070"),
            AvailableQuantity = 40
        },
        new ProductStock
        {
            ProductId = Guid.Parse("80808080-8080-8080-8080-808080808080"),
            AvailableQuantity = 50
        },
        new ProductStock
        {
            ProductId = Guid.Parse("90909090-9090-9090-9090-909090909090"),
            AvailableQuantity = 60
        }
    ];   
}
