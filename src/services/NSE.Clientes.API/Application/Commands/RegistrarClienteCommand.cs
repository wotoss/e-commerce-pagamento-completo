
using FluentValidation;
using NSE.Core.Messages;
using System;

namespace NSE.Clientes.API.Application.Commands
{
    //vai representar o transporte de dados de um cliente até a sua persistencia no base de dados.
    public class RegistrarClienteCommand : Command
    {
        public Guid Id { get; private set; }

        public string Nome { get; private set; }

        public string Email { get; private set; }

        public string Cpf { get; private set; }

        public RegistrarClienteCommand(Guid id, string nome, string email, string cpf)
        {
            AggregateId = id;//aqui eu passe o  (AggregateId) recebendo o mesmo id. Devido eu esta utilizando como herança a (classe Message).
            Id = id;
            Nome = nome;
            Email = email;
            Cpf = cpf;
        }

        //vamos sobrescrever o método EhValido. Lembrando que ele foi criado como virtual, tem que sobescrever
        public override bool EhValido()
        {
            //vai receber a instancia new RegistrarClienteValidation Validando (Validate => (o proprio cliente => (this)
            ValidationResult = new RegistrarClienteValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        //ESTA CLASSE ESTA COMO UM A CLASSE ANINHADA UMA CLASSE(RegistrarClienteCommand) DENTRO DE OUTRA. (RegistrarClienteValidation).


        //Aqui estou dizendo que vou construir as minhas regras de validação baseado no (AbstractValidator)
        public class RegistrarClienteValidation : AbstractValidator<RegistrarClienteCommand>
        {
            public RegistrarClienteValidation()
            {
                //estou fazendo a regra de todo a minha classe (RegistrarClienteCommand) usando como biblioteca FluentValidation.AbstractValidator
                RuleFor(c => c.Id)
                    .NotEqual(Guid.Empty)//estou colocando em minha regra que o Id não pode ser vázio.
                    .WithMessage("Id do cliente inválido");

                RuleFor(c => c.Nome)
                    .NotEmpty() //Não pode estar vazio
                    .WithMessage("O nome do cliente não foi informado");

                RuleFor(c => c.Cpf)
                    .Must(TerCpfValido)
                    .WithMessage("O cpf informado não é válido.");

                RuleFor(c => c.Email)
                    .Must(TerEmailValido)
                    .WithMessage("O e-mail informado não é válido.");
            }

            protected static bool TerCpfValido(string cpf)
            {
                return Core.DomainObjects.Cpf.Validar(cpf);
            }

            protected static bool TerEmailValido(string email)
            {
                //estou percorrendo o caminho (Core.DomainObjects) até chegar na classe Email e trazer o método Validar(cpf)
                //faço isto com o método de acesso (.)
                return Core.DomainObjects.Email.Validar(email);
            }
        }
    }
}

//using System;
//using FluentValidation;
//using NSE.Core.Messages;

//namespace NSE.Clientes.API.Application.Commands
//{
//    public class RegistrarClienteCommand : Command
//    {
//        public Guid Id { get; private set; }
//        public string Nome { get; private set; }
//        public string Email { get; private set; }
//        public string Cpf { get; private set; }

//        public RegistrarClienteCommand(Guid id, string nome, string email, string cpf)
//        {
//            AggregateId = id;
//            Id = id;
//            Nome = nome;
//            Email = email;
//            Cpf = cpf;
//        }

//        public override bool EhValido()
//        {
//            ValidationResult = new RegistrarClienteValidation().Validate(this);
//            return ValidationResult.IsValid;
//        }

//        public class RegistrarClienteValidation : AbstractValidator<RegistrarClienteCommand>
//        {
//            public RegistrarClienteValidation()
//            {
//                RuleFor(c => c.Id)
//                    .NotEqual(Guid.Empty)
//                    .WithMessage("Id do cliente inválido");

//                RuleFor(c => c.Nome)
//                    .NotEmpty()
//                    .WithMessage("O nome do cliente não foi informado");

//                RuleFor(c => c.Cpf)
//                    .Must(TerCpfValido)
//                    .WithMessage("O CPF informado não é válido.");

//                RuleFor(c => c.Email)
//                    .Must(TerEmailValido)
//                    .WithMessage("O e-mail informado não é válido.");
//            }

//            protected static bool TerCpfValido(string cpf)
//            {
//                return Core.DomainObjects.Cpf.Validar(cpf);
//            }

//            protected static bool TerEmailValido(string email)
//            {
//                return Core.DomainObjects.Email.Validar(email);
//            }
//        }
//    }
//}