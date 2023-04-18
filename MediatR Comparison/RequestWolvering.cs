using MediatR;

namespace MediatR_Comparison
{
    public record RequestWolvering(string Something): IRequest<string>;
    public class RequestWolveringHandler
    {
        public string Handle(RequestWolvering request) => $"Hello world {request.Something}";
    }
}
