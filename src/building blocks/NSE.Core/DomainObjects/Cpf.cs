

using NSE.Core.Utils;
using System.Text.RegularExpressions;

namespace NSE.Core.DomainObjects
{
    //Esta é uma classe objeto de valor
    public class Cpf
    {
        //O Cpf tem uma constante que indica o tamanho máximo do (cpf)
        public const int CpfMaxLength = 11;

        public string Numero { get; private set; }

        //Vai ter um construtor protegido do EntityFramework
        protected Cpf() { }

        //Já neste contrutor passamos o numero do Cpf onde ele ira seta Numero da classe
        public Cpf(string numero)
        {
            //usando a classe que eu criei (DomainException)
            if (!Validar(numero)) throw new DomainException("CPF inválido");
            //estamos setando o Numero da entidade ou classe
            Numero = numero;
        }

        public static bool Validar(string cpf)
        {
            //Eu construi uma validação para validar se esta vindo apenas numero no meu cpf sem caracters
            cpf = cpf.ApenasNumeros(cpf);

            if (cpf.Length > 11)
                return false;

            while (cpf.Length != 11)
                cpf = '0' + cpf;

            var igual = true;
            for (var i = 1; i < 11 && igual; i++)
                if (cpf[i] != cpf[0])
                    igual = false;

            if (igual || cpf == "12345678909")
                return false;

            var numeros = new int[11];

            for (var i = 0; i < 11; i++)
                numeros[i] = int.Parse(cpf[i].ToString());

            var soma = 0;
            for (var i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            var resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else if (numeros[10] != 11 - resultado)
                return false;

            return true;
        }
    }
}
