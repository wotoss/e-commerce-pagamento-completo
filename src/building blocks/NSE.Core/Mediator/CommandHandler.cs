using FluentValidation.Results;

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
    }
}
