using System;
using System.Collections.Generic;
using System.Text;

namespace NSE.Core.Messages.Integration
{
    //esta classe está no passado o usuário já foi registrado
    //Lembrando que esta é a mensagem que será o meu request ela (vai) vou criar uma classe que volta msg será ResponseMessage
    public class UsuarioRegistradoIntegrationEvent : IntegrationEvent
    {
        //quando eu tenho o set privado eu uso construtor
        public Guid Id { get; private set; }

        public string Nome { get; private set; }

        public string Email { get; private set; }

        public string Cpf { get; private set; }

        public UsuarioRegistradoIntegrationEvent(Guid id, string nome, string email, string cpf)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Cpf = cpf;
        }
    }
}
