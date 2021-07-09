using System;
using NSE.Core.DomainObjects;

namespace NSE.Core.Data
{
    //O IAggregateRoot é apenas uma interface de (Marcação) não tem nada dentro mas pode ser usada.
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        
    }
}