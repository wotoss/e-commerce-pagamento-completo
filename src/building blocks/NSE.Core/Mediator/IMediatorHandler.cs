using FluentValidation.Results;
using NSE.Core.Messages;
using System.Threading.Tasks;

namespace NSE.Core.Mediator
{
    public interface IMediatorHandler
    {
        //veja que (T evento) ( where => obriga) herdar de (Event)
        Task PublicarEvento<T>(T evento) where T : Event;

        //metodo (EnviarComando) onde ele será um (T comando) e tem que herdar de (Command)
        Task<ValidationResult> EnviarComando<T>(T comando) where T : Command;
    }
}
