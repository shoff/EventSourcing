
namespace Domain.Commands
{
    using MediatR;

    public interface ICommandHandler<in T>: IRequestHandler<T>
        where T : ICommand
    {
    }
}
