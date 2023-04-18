using MediatR;
using MediatR_Comparison;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(r => r.RegisterServicesFromAssemblyContaining<QueryMediatR>());

builder.Host.UseWolverine();

var app = builder.Build();

app.MapGet("/mediatr", (IMediator mediator) => mediator.Send(new RequestMediatR(
    Guid.NewGuid().ToString())));

app.MapGet("/wolvering", (IMessageBus bus) => bus.InvokeAsync<string>(
    new RequestWolvering(Guid.NewGuid().ToString())));

app.Run();
