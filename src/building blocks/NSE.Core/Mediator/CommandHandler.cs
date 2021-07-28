using FluentValidation.Results;
using NSE.Core.Data;
using System.Threading.Tasks;

namespace NSE.Core.Mediator
{
   public abstract class CommandHandler
    {
        //Muito bom ! Eu fiz a injeção por dependencia e criei uma instância de (ValidationResult)
        protected ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        //quando a mensagem de erro vir atraves do parametro (string mensagem)
        //eu vou adicionar ela em minha coleção ( ValidationResult.Errors.Add)
        //o qual eu crie a instancia na injeção de dependencia
        protected void AdicionarErro(string mensagem)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        }


        //fiz um método PersistirDados (com a minha interface => IUnitOfWork, desta forma eu trago o metodo Commit )
        //e digo se for diferente da implementação base  AdiconeErro que seria para adicionar a mensagem.
        //Caso não der o commit enviará a mensagem de erro.
        protected async Task<ValidationResult> PersistirDados(IUnitOfWork uow)
        {
            if (!await uow.Commit()) AdicionarErro("Houve um erro ao persistir os dados");

            return ValidationResult;
        }
    }
}
