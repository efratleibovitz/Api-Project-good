using OrderConsumer;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<KafkaConsumerService>();
    })
    .Build();

await host.RunAsync();
