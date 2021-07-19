using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSE.Core.Utils
{
   public static class StringUtils
    {
        //Ele vai devolver apenas os numeros deste cpf
        public static string ApenasNumeros(this string str, string input)
        {
            //ele pega a (string) faz um (where = usando link) e ai devolve apenas os numeros deste (cpf)
            return new string(input.Where(char.IsDigit).ToArray());
        }
    }
}
