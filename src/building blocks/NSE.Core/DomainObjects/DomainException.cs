using System;


namespace NSE.Core.DomainObjects
{
    //Esta é uma classe de excessão, onde vou tratar os meus erros.
    //Temos a implementação padrão desta  classe DomainException
    public class DomainException : Exception
    {
        public DomainException() { }

        public DomainException(string message) : base(message) { }

        public DomainException(string message, Exception innerException) : base(message, innerException) { }

    }

    //Uma observação importante. 
    //Não é bom ficar soltando Exception pelo sistema. Ele para a aplicação.
    //Temos outras validações que podem serfeitas em cpf por exemplo 
}
