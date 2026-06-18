using KafkaConsumerLogger;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<OrderTransactionConsumerService>();
    })
    .Build();

await host.RunAsync();
