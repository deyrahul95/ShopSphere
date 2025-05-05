using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.DB;

public class InMemoryDB
{
    public readonly static List<Product> Products = [
        new Product
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Samsung S23 Lavender",
            Category = "Smartphone",
            Description = "8GB RAM, 128 GB ROM, Qualcomm Snapdragon 8 Gen 2 processor, 6.1 Inch Super Amoled Display",
            Price = 39980.35M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Name = "Apple iPhone 14 Pro",
            Category = "Smartphone",
            Description = "6GB RAM, 128GB Storage, A16 Bionic chip, 6.1-inch Super Retina XDR display",
            Price = 99999.99M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Name = "Google Pixel 7",
            Category = "Smartphone",
            Description = "8GB RAM, 128GB Storage, Google Tensor G2 chip, 6.3-inch OLED display",
            Price = 59999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            Name = "Dell XPS 13",
            Category = "Laptop",
            Description = "Intel Core i7, 16GB RAM, 512GB SSD, 13.4-inch FHD+ display",
            Price = 129999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            Name = "Apple MacBook Air M2",
            Category = "Laptop",
            Description = "Apple M2 chip, 8GB RAM, 256GB SSD, 13.6-inch Retina display",
            Price = 99999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            Name = "HP Spectre x360",
            Category = "Laptop",
            Description = "Intel Core i7, 16GB RAM, 1TB SSD, 13.3-inch 4K OLED display",
            Price = 149999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
            Name = "Microsoft Surface Pro 9",
            Category = "Tablet",
            Description = "Intel Core i5, 8GB RAM, 256GB SSD, 13-inch PixelSense display",
            Price = 89999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
            Name = "Apple iPad Pro 11",
            Category = "Tablet",
            Description = "M1 chip, 8GB RAM, 128GB Storage, 11-inch Liquid Retina display",
            Price = 79999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            Name = "Samsung Galaxy Tab S8",
            Category = "Tablet",
            Description = "8GB RAM, 128GB Storage, Snapdragon 8 Gen 1, 11-inch display",
            Price = 64999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Name = "LG UltraFine 5K Monitor",
            Category = "Monitor",
            Description = "27-inch 5K display, Thunderbolt 3, P3 wide color gamut",
            Price = 129999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Name = "Dell UltraSharp U2720Q",
            Category = "Monitor",
            Description = "27-inch 4K UHD, IPS technology, USB-C connectivity",
            Price = 59999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            Name = "ASUS ROG Swift PG32UQ",
            Category = "Monitor",
            Description = "32-inch 4K UHD, 144Hz, G-SYNC compatible",
            Price = 79999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
            Name = "Logitech MX Master 3",
            Category = "Mouse",
            Description = "Wireless mouse with ergonomic design, customizable buttons",
            Price = 9999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
            Name = "Razer DeathAdder V2",
            Category = "Mouse",
            Description = "Gaming mouse with 20,000 DPI optical sensor, ergonomic design",
            Price = 4999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
            Name = "Apple Magic Mouse 2",
            Category = "Mouse",
            Description = "Wireless mouse with multi-touch surface, rechargeable battery",
            Price = 7999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("10101010-1010-1010-1010-101010101010"),
            Name = "Sony WH-1000XM5",
            Category = "Headphones",
            Description = "Noise-canceling headphones, 30-hour battery life, USB-C charging",
            Price = 29999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("20202020-2020-2020-2020-202020202020"),
            Name = "Bose QuietComfort 45",
            Category = "Headphones",
            Description = "Noise-canceling, up to 24 hours of battery life, comfortable design",
            Price = 24999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("30303030-3030-3030-3030-303030303030"),
            Name = "JBL Tune 125TWS",
            Category = "Headphones",
            Description = "True wireless earbuds, 32-hour battery, Bluetooth 5.0",
            Price = 5999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("40404040-4040-4040-4040-404040404040"),
            Name = "Fossil Gen 5 Smartwatch",
            Category = "Smartwatch",
            Description = "Wear OS by Google, Heart rate monitoring, GPS, and sleep tracking",
            Price = 17999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("50505050-5050-5050-5050-505050505050"),
            Name = "Apple Watch Series 7",
            Category = "Smartwatch",
            Description = "Fitness tracking, Blood Oxygen monitoring, 40mm Retina display",
            Price = 42999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("60606060-6060-6060-6060-606060606060"),
            Name = "Samsung Galaxy Watch 5",
            Category = "Smartwatch",
            Description = "Health tracking, Sleep monitoring, 1.4-inch Super AMOLED display",
            Price = 24999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("70707070-7070-7070-7070-707070707070"),
            Name = "Acer Predator Helios 300",
            Category = "Gaming Laptop",
            Description = "Intel i7-11800H, 16GB RAM, RTX 3060, 15.6-inch Full HD display",
            Price = 129999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("80808080-8080-8080-8080-808080808080"),
            Name = "Alienware M15 R6",
            Category = "Gaming Laptop",
            Description = "Intel Core i7, 16GB RAM, RTX 3070, 15.6-inch 165Hz display",
            Price = 159999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("90909090-9090-9090-9090-909090909090"),
            Name = "Razer Blade 15",
            Category = "Gaming Laptop",
            Description = "Intel Core i9, 32GB RAM, RTX 3080, 15.6-inch 4K OLED display",
            Price = 219999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("10101010-1010-1010-1010-101010101010"),
            Name = "Corsair K95 RGB Platinum XT",
            Category = "Keyboard",
            Description = "Mechanical keyboard with RGB lighting, programmable keys",
            Price = 15999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("20202020-2020-2020-2020-202020202020"),
            Name = "Logitech G Pro X",
            Category = "Keyboard",
            Description = "Mechanical keyboard with RGB lighting, 100% programmable keys",
            Price = 10999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("30303030-3030-3030-3030-303030303030"),
            Name = "Razer Huntsman Mini",
            Category = "Keyboard",
            Description = "60% compact mechanical keyboard with RGB lighting",
            Price = 7999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("40404040-4040-4040-4040-404040404040"),
            Name = "Logitech G Pro Wireless",
            Category = "Mouse",
            Description = "Lightweight wireless gaming mouse, 16,000 DPI",
            Price = 7999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("50505050-5050-5050-5050-505050505050"),
            Name = "SteelSeries Rival 600",
            Category = "Mouse",
            Description = "Gaming mouse with dual sensors, 12,000 DPI",
            Price = 6999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("60606060-6060-6060-6060-606060606060"),
            Name = "Corsair Dark Core RGB/SE",
            Category = "Mouse",
            Description = "Wireless gaming mouse, 16,000 DPI, RGB lighting",
            Price = 8999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("70707070-7070-7070-7070-707070707070"),
            Name = "Sennheiser Momentum 3",
            Category = "Headphones",
            Description = "Noise-canceling headphones with 17-hour battery life, premium sound quality",
            Price = 29999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("80808080-8080-8080-8080-808080808080"),
            Name = "Sony Noise Cancelling WH-1000XM4",
            Category = "Headphones",
            Description = "Industry-leading noise cancellation, 30 hours of battery life",
            Price = 26999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("90909090-9090-9090-9090-909090909090"),
            Name = "Bose SoundLink Revolve+",
            Category = "Speaker",
            Description = "360-degree sound, portable Bluetooth speaker with 16 hours of battery life",
            Price = 22999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("10101010-1010-1010-1010-101010101010"),
            Name = "JBL Flip 5",
            Category = "Speaker",
            Description = "Portable Bluetooth speaker, 12 hours of battery life",
            Price = 9999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product
        {
            Id = Guid.Parse("20202020-2020-2020-2020-202020202020"),
            Name = "Marshall Stanmore II",
            Category = "Speaker",
            Description = "Bluetooth speaker with classic Marshall design, 20+ hours of battery life",
            Price = 22999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }
    ];
}
