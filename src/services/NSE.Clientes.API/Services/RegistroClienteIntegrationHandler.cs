using EasyNetQ;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.Clientes.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.Core.Messages.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Clientes.API.Services
{
    //Ele vai manipular a integração. => este BackgroundService ou fundos ele fica ativo
    //mas ele só execulta quando recebe a informação.
    public class RegistroClienteIntegrationHandler : BackgroundService
    {
        //fazendo a injeção de dependencia do meu IBus ele que faz o transporte das informações.
        //Neste caso o response.
        private IBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public RegistroClienteIntegrationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //aqui eu tenho um bus criado => estou passando a ConnectionString => do meu Rabbitmq  (host=localhost:5672) está configurado no docker
            _bus = RabbitHutch.CreateBus("host=localhost:5672");
            //RespondAsync vou esperar por uma classe => RegistroClienteIntegrationHandler e responder => ResponseMessage
            _bus.Rpc.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request =>
            new ResponseMessage(await RegistrarCliente(request)));

            //acabou a tarefa.
            return Task.CompletedTask;
        }

        //Aqui nés fizemos um processo diferente da injeção de dependencia
        private async Task<ValidationResult> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
        {
            var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);
            ValidationResult sucesso;
            //utilizado para resolver serviços dentro o scoped
            using (var scope = _serviceProvider.CreateScope())
            {
                //esta abordagem é chamada de serviceLocation => substitui neste caso a injeção de dependência
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();

                //declarei o sucesso na linha 45 do lado de fora do escopo.
                sucesso = await mediator.EnviarComando(clienteCommand);
            }
            return sucesso;
        }
    }
}
