using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Cache.CacheManager;
using Steeltoe.Discovery.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddDiscoveryClient(builder.Configuration);

builder.Services.AddOcelot()
    .AddConsul()
    .AddCacheManager(x => x.WithDictionaryHandle()); ;

var app = builder.Build();

app.UseOcelot().Wait();

await app.RunAsync();