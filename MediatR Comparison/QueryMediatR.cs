using MediatR;

public record RequestMediatR(string Something): IRequest<string>;
public class QueryMediatR : IRequestHandler<RequestMediatR, string>
{
    public Task<string> Handle(RequestMediatR request, CancellationToken cancellationToken) =>
        Task.FromResult($"Hello World  {request.Something}");
    
}