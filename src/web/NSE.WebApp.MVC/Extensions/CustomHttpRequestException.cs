using System;
using System.Net;


namespace NSE.WebApp.MVC.Extensions
{
    public class CustomHttpRequestException : Exception
    {
        public HttpStatusCode StatusCode;

        //posso contrui sem passar nenhum parametro
        public CustomHttpRequestException () { }

        //posso passar parametro da informação real, (message, innerException)
        public CustomHttpRequestException(string message, Exception innerException)
        //ele ira retorna para minha (base) que é a extensão (:Excpetion).
            : base(message, innerException) { }

        //NA VERDADE NÃO PRECISAMOS IMPLEMENTAR OS DOIS ACIMA = MAS É UMA BOA PRÁTICA = A FINAL USAR ESTE (StatusCode)
        //Ou se você quiser pode criar uma instância dela como o statuCode
        public CustomHttpRequestException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

    }
}
