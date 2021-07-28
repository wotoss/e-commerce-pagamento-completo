using System.Collections.Generic;
using System.Threading.Tasks;
using NSE.Core.Data;

namespace NSE.Clientes.API.Models
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        //para adicinar clientes
        void Adicionar(Cliente cliente);

        //retornar uma lista de clientes
        Task<IEnumerable<Cliente>> ObterTodos();

        //Obter o cliente por Cpf, para poder validar se o o cliente já existi ou não na base de dados.
        Task<Cliente> ObterPorCpf(string cpf);
    }
}