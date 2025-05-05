using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.DB;

public class InMemoryDB
{
    public readonly static List<Product> Products = [
        new Product ()
        {
            Id = Guid.NewGuid(),
            Name = "Samsung S23 Lavender",
            Category = "Smartphone",
            Description = "8GB RAM, 128 GB ROM, Qualcomm Snapdragon 8 Gen 2 processor, 6.1 Inch Super Amoled Display",
            Price = 39980.35M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Apple iPhone 14 Pro",
            Category = "Smartphone",
            Description = "6GB RAM, 128GB Storage, A16 Bionic chip, 6.1-inch Super Retina XDR display",
            Price = 99999.99M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Google Pixel 7",
            Category = "Smartphone",
            Description = "8GB RAM, 128GB Storage, Google Tensor G2 chip, 6.3-inch OLED display",
            Price = 59999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Dell XPS 13",
            Category = "Laptop",
            Description = "Intel Core i7, 16GB RAM, 512GB SSD, 13.4-inch FHD+ display",
            Price = 129999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Apple MacBook Air M2",
            Category = "Laptop",
            Description = "Apple M2 chip, 8GB RAM, 256GB SSD, 13.6-inch Retina display",
            Price = 99999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "HP Spectre x360",
            Category = "Laptop",
            Description = "Intel Core i7, 16GB RAM, 1TB SSD, 13.3-inch 4K OLED display",
            Price = 149999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Microsoft Surface Pro 9",
            Category = "Tablet",
            Description = "Intel Core i5, 8GB RAM, 256GB SSD, 13-inch PixelSense display",
            Price = 89999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Apple iPad Pro 11",
            Category = "Tablet",
            Description = "M1 chip, 8GB RAM, 128GB Storage, 11-inch Liquid Retina display",
            Price = 79999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Samsung Galaxy Tab S8",
            Category = "Tablet",
            Description = "8GB RAM, 128GB Storage, Snapdragon 8 Gen 1, 11-inch display",
            Price = 64999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "LG UltraFine 5K Monitor",
            Category = "Monitor",
            Description = "27-inch 5K display, Thunderbolt 3, P3 wide color gamut",
            Price = 129999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Dell UltraSharp U2720Q",
            Category = "Monitor",
            Description = "27-inch 4K UHD, IPS technology, USB-C connectivity",
            Price = 59999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "ASUS ROG Swift PG32UQ",
            Category = "Monitor",
            Description = "32-inch 4K UHD, 144Hz, G-SYNC compatible",
            Price = 79999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Logitech MX Master 3",
            Category = "Mouse",
            Description = "Wireless mouse with ergonomic design, customizable buttons",
            Price = 9999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Razer DeathAdder V2",
            Category = "Mouse",
            Description = "Gaming mouse with 20,000 DPI optical sensor, ergonomic design",
            Price = 4999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Apple Magic Mouse 2",
            Category = "Mouse",
            Description = "Wireless mouse with multi-touch surface, rechargeable battery",
            Price = 7999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Bose SoundLink Revolve+",
            Category = "Speaker",
            Description = "Portable Bluetooth speaker with 360-degree sound",
            Price = 24999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "JBL Charge 5",
            Category = "Speaker",
            Description = "Portable Bluetooth speaker with powerful sound and power bank",
            Price = 19999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Sonos One",
            Category = "Speaker",
            Description = "Smart speaker with voice control and rich sound",
            Price = 19999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Apple AirPods Pro",
            Category = "Headphone",
            Description = "Active noise cancellation, transparency mode, sweat and water resistance",
            Price = 24999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Samsung Galaxy Buds Pro",
            Category = "Headphones",
            Description = "Active noise cancellation, 28 hours of battery life, IPX7 water resistance",
            Price = 19999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Sony WH-1000XM4",
            Category = "Headphones",
            Description = "Wireless noise-canceling headphones with touch sensor controls",
            Price = 29999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Microsoft Surface Laptop 4",
            Category = "Laptop",
            Description = "Intel Core i5, 8GB RAM, 512GB SSD, 13.5-inch PixelSense touchscreen",
            Price = 109999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Acer Aspire 5",
            Category = "Laptop",
            Description = "AMD Ryzen 5, 8GB RAM, 512GB SSD, 15.6-inch FHD display",
            Price = 49999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Lenovo ThinkPad X1 Carbon",
            Category = "Laptop",
            Description = "Intel Core i7, 16GB RAM, 1TB SSD, 14-inch FHD display",
            Price = 139999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Samsung Galaxy Tab A7",
            Category = "Tablet",
            Description = "3GB RAM, 64GB Storage, 10.4-inch display, Dolby Atmos speakers",
            Price = 24999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Microsoft Surface Go 3",
            Category = "Tablet",
            Description = "Intel Pentium Gold, 4GB RAM, 64GB Storage, 10.5-inch touchscreen",
            Price = 49999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "BenQ PD3220U",
            Category = "Monitor",
            Description = "32-inch 4K UHD, HDR10, USB-C connectivity, designed for designers",
            Price = 79999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Acer Predator X27",
            Category = "Monitor",
            Description = "27-inch 4K UHD, 144Hz, G-SYNC Ultimate, HDR support",
            Price = 149999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Logitech G502 HERO",
            Category = "Mouse",
            Description = "Wired gaming mouse with customizable RGB lighting and 25,600 DPI",
            Price = 5999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Corsair Dark Core RGB SE",
            Category = "Mouse",
            Description = "Wireless gaming mouse with customizable RGB lighting and 16,000 DPI",
            Price = 7999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Ultimate Ears BOOM 3",
            Category = "Speaker",
            Description = "Portable Bluetooth speaker with 360-degree sound and waterproof design",
            Price = 14999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Anker Soundcore 2",
            Category = "Speaker",
            Description = "Portable Bluetooth speaker with 24-hour battery life and IPX7 waterproof",
            Price = 6999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Beats Studio3 Wireless",
            Category = "Headphones",
            Description = "Wireless noise-canceling headphones with up to 22 hours of battery life",
            Price = 24999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Nike Air Max 270",
            Category = "Footwear",
            Description = "Men's running shoes with a stylish design and comfortable cushioning.",
            Price = 12999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Adidas Ultraboost 21",
            Category = "Footwear",
            Description = "High-performance running shoes with responsive cushioning and support.",
            Price = 15999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Levi's 501 Original Jeans",
            Category = "Apparel",
            Description = "Classic straight-fit jeans made from durable denim.",
            Price = 4999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "The North Face Borealis Backpack",
            Category = "Accessories",
            Description = "Versatile backpack with ample storage and comfortable fit.",
            Price = 8999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Fossil Gen 5 Smartwatch",
            Category = "Watches",
            Description = "Stylish smartwatch with fitness tracking and customizable watch faces.",
            Price = 19999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "KitchenAid Artisan Stand Mixer",
            Category = "Home Appliances",
            Description = "Powerful stand mixer with multiple attachments for versatile cooking.",
            Price = 24999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Instant Pot Duo 7-in-1",
            Category = "Home Appliances",
            Description = "Multi-cooker that functions as a pressure cooker, slow cooker, and more.",
            Price = 12999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Yeti Rambler 20 oz Tumbler",
            Category = "Drinkware",
            Description = "Durable stainless steel tumbler with double-wall vacuum insulation.",
            Price = 2999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Bose QuietComfort 35 II",
            Category = "Headphones",
            Description = "Wireless noise-canceling headphones with Alexa voice control.",
            Price = 29999.00M,
            InStock = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Oster Blender Pro 1200",
            Category = "Kitchen Appliances",
            Description = "Powerful blender with 7 speeds and a 1200-watt motor.",
            Price = 8999.00M,
            InStock = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }
    ];
}
