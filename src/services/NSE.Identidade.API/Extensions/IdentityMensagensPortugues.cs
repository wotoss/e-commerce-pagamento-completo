

using Microsoft.AspNetCore.Identity;

namespace NSE.Identidade.API.Extensions
{
    public class IdentityMensagensPortugues : IdentityErrorDescriber
    {
        public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = "Ocorreu um erro desconhecido." }; }

        public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Falha de concorrência otimista, o objeto foi modificado." }; }

        public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = "Senha Incorreta." }; }

        public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = "Token Inválido." }; }

        public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Já existe um usuário com este login." }; }

        public override IdentityError InvalidUserName(string userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = $"O login '{userName}' é inválido pode conter apenas letras " } ; }

        public override IdentityError InvalidEmail(string email) { return new IdentityError { Code = nameof(InvalidEmail), Description = $"O email '{email}' é inválido " } ; }

        public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = $"O login '{userName}' já esta sendo utilizado." }; }

        public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = $"O email '{email}' já esta sendo utilizado." }; }

        public override IdentityError InvalidRoleName(string role) { return new IdentityError { Code = nameof(InvalidRoleName), Description = $"A permissão '{role}' é inválida." }; }

        public override IdentityError DuplicateRoleName(string role) { return new IdentityError { Code = nameof(DuplicateRoleName), Description = $"A permissão '{role}' já está utilizada." }; }

        public override IdentityError UserAlreadyHasPassword() { return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = $"O usuário já possui uma senha definida." }; }

        public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "O lockout não está habilitado para este usuário." }; }

        public override IdentityError UserAlreadyInRole(string role) { return new IdentityError {Code = nameof(UserAlreadyInRole), Description = $"O usuario já possui a permissão '{role}' " }; }

        public override IdentityError UserNotInRole(string role) { return new IdentityError { Code = nameof(UserNotInRole), Description = $"O usuário não tem a permissão '{role}' " }; }

        public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = $"A senha deve conter ao menos '{length}' caracteres." }; }
        







    }
}
