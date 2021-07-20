
using FluentValidation.Results;
using MediatR;
using NSE.Clientes.API.Models;
using NSE.Core.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Clientes.API.Application.Commands
{
    //IRequestHandler é manipulador do Request
    public class ClienteCommandHandler : CommandHandler,
        IRequestHandler<RegistrarClienteCommand, ValidationResult>
    {
        //este (message é o request)
        public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;

            //criei uma instancia de cliente e passei as informações desejadas
            var cliente = new Cliente(message.Id, message.Nome, message.Email, message.Cpf);
            
            //Validações de negocio

            //Persistir no banco !
            if (true) //Se acaso já existir cliente com o CPF informado.
            {
                AdicionarErro("Este CPF já está e uso.");
                return ValidationResult;
            }
            
            //return message.ValidationResult;
        }

    }
}



//using FluentValidation.Results;
//using MediatR;
//using System.Threading;
//using System.Threading.Tasks;
//using NSE.Clientes.API.Application.Events;
//using NSE.Clientes.API.Models;
//using NSE.Core.Messages;
//using System.ComponentModel.DataAnnotations;

//namespace NSE.Clientes.API.Application.Commands
//{
//    public class ClienteCommandHandler : CommandHandler,
//        IRequestHandler<RegistrarClienteCommand, ValidationResult>
//    {
//        private readonly IClienteRepository _clienteRepository;

//        public ClienteCommandHandler(IClienteRepository clienteRepository)
//        {
//            _clienteRepository = clienteRepository;
//        }
        
//        public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
//        {
//            if (!message.EhValido()) return message.ValidationResult;

//            var cliente = new Cliente(message.Id, message.Nome, message.Email, message.Cpf);

//            var clienteExistente = await _clienteRepository.ObterPorCpf(cliente.Cpf.Numero);

//            if (clienteExistente != null)
//            {
//                AdicionarErro("Este CPF já está em uso.");
//                return ValidationResult;
//            }

//            _clienteRepository.Adicionar(cliente);

//            cliente.AdicionarEvento(new ClienteRegistradoEvent(message.Id, message.Nome, message.Email, message.Cpf));

//            return await PersistirDados(_clienteRepository.UnitOfWork);
//        }
//    }
//}