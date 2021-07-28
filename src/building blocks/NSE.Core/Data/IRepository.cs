using System;
using NSE.Core.DomainObjects;

namespace NSE.Core.Data
{
    //O IAggregateRoot é apenas uma interface de (Marcação) não tem nada dentro mas pode ser usada.
    //Quero dizer não tinha nada, agora eu adicionei o (IUnitOfWork)
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        //neste momento vou implementar o IUnitOfWork que faz a persistencia o meu commit
        IUnitOfWork UnitOfWork { get; }
    }
}