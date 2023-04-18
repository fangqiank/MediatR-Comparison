using Marten;
using Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Oakton;
using Wolverine;
using Wolverine.AmazonSqs;
using Wolverine.Transports.Tcp;

await Host.CreateDefaultBuilder(args)
    //.UseWolverine(opts =>
    //{
       
    //    opts.UseAmazonSqsTransport(ao =>
    //    {
    //        ao.ServiceURL = "https://sqs.eu-north-1.amazonaws.com";
    //    }).AutoProvision();

    //    opts.ListenToSqsQueue("pongs-queue");
    //});

    .UseWolverine(opts => opts.ListenAtPort(8011))
    .RunOaktonCommands(args);

public class PingHandler
{
    public ValueTask Handle(
        Ping ping,
        ILogger<PingHandler> logger,
        IMessageContext context
        )
    {
        logger.LogInformation("Got Ping, #{Number}", ping.Number);

        return context.RespondToSenderAsync(new Pong(ping.Number));
    }
}