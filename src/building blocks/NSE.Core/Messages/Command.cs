using FluentValidation.Results;
using MediatR;
using System;

//esta classe command será a minha classe base
namespace NSE.Core.Messages
{
    //alem de ser base é uma classe abstrata => só pode ser herdada e não implementada.
    //usando o (IRequest) ele pode retornar ou não um requeste e response. Mas eu que que ele retorne <ValidationResult>
    public abstract class Command : Message, IRequest<ValidationResult>
    {
        //Timestamp porque terá um hórario de um dia ou uma data.
        public  DateTime Timestamp { get; private set; }

        public ValidationResult ValidationResult { get; set; }

        //vou dizer aqui no meu construtor que a minha propriedade timestamp tem o valor do meu DateTime.Now (que seria a data e horario atual)
        protected Command()
        {
            Timestamp = DateTime.Now;
        }


        //por ele ser virtual eu posso quere dar um (override)
        //e se eu dar o EhValido sem dar o command ele vai cair na exception
        //Resumindo => eu não vou conseguir valida algo que não foi implementado.
        public virtual bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}
