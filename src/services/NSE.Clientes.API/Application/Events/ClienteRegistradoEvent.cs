﻿
using NSE.Core.Messages;
using System;

namespace NSE.Clientes.API.Application.Events
{ 

        //Lembre-se está classe herda de Event que seria algo já aconteceu. 
        //Veja o nome ClienteRegistrado
        public class ClienteRegistradoEvent : Event
        {
        //Esta são informações da propria entidade
               public Guid Id { get; private set; }

               public string Nome { get; private set; }

               public string Email { get; private set; }

               public string Cpf { get; private set; }

        //construtor recebendo as mesmas informações.
        public ClienteRegistradoEvent(Guid id, string nome, string email, string cpf)
        {
            AggregateId = id;
            Id = id;
            Nome = nome;
            Email = email;
            Cpf = cpf;

        }
    }

}

//{
//    public class ClienteRegistradoEvent : Event
//    {
//        public Guid Id { get; private set; }
//        public string Nome { get; private set; }
//        public string Email { get; private set; }
//        public string Cpf { get; private set; }

//        public ClienteRegistradoEvent(Guid id, string nome, string email, string cpf)
//        {
//            AggregateId = id;
//            Id = id;
//            Nome = nome;
//            Email = email;
//            Cpf = cpf;
//        }
//    }
//}