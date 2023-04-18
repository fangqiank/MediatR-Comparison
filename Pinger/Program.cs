using Marten;
using Messages;
using Oakton.Resources;
using Wolverine;
using Wolverine.AmazonSqs;
using Wolverine.Attributes;
using Wolverine.Marten;
using Wolverine.Transports.Tcp;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine(opts =>
{
    
    opts.Services.AddMarten("host=192.168.1.12;port=5432;database=wolvering_demo;user id=postgres;password=g73gle73;")
        .IntegrateWithWolverine();


    //opts.UseAmazonSqsTransport(ao =>
    //{
    //    ao.ServiceURL = "https://sqs.eu-north-1.amazonaws.com";
    //})
    //.AutoProvision();

    //opts.ListenToSqsQueue("pongs-queue")
    //    .UseDurableInbox();

    //opts.ListenToSqsQueue("pongs-queue");

    //opts.PublishMessage<Ping>().ToSqsQueue("pings-queue")
    //   .UseDurableOutbox();
    //opts.PublishMessage<Ping>().ToSqsQueue("pings-queue");

    opts.ListenAtPort(8010).UseDurableInbox();

    opts.PublishMessage<Ping>().ToPort(8011).UseDurableOutbox();
})
    .UseResourceSetupOnStartup();

var app = builder.Build();

app.MapGet("/send", async (IMessageBus bus) => 
    await bus.PublishAsync(new Ping(0)));

app.Run();

public class PongRecord
{
    public int Id { get; set; }
    public Pong Message { get; set; }
}

public class PongHandler
{
    [Transactional]
    public async Task Handle(
        Pong pong,
        ILogger<PongHandler> logger,
        IMessageBus bus,
        IDocumentSession session
        )
    {
        logger.LogInformation("Received Pong #{Number}", pong.Number);

        session.Store(new PongRecord { Message = pong });

        if (pong.Number < 3)
            await bus.SendAsync(new Ping(pong.Number + 1));
        else
            logger.LogInformation("Received Pong #{Number}, finishing", pong.Number);

       await session.SaveChangesAsync();
    }
}
