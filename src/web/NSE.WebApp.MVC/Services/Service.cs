using NSE.WebApp.MVC.Extensions;

using System.Net.Http;


namespace NSE.WebApp.MVC.Services
{
    //Classe abstrata ela só pode ser herdade e não instânciada 
    public abstract  class Service
    {
        protected bool TratarErrosResponse (HttpResponseMessage response)
        {
            //O StatusCode me diz qual foi o tipo de erro que aconteceu se foi um 500, 404 ...
            switch ((int)response.StatusCode)
            {
                //erro 401 ele não conhece o usuário.
                case 401:
                //Acesso negado
                case 403:
                //recuros não encontrado
                case 404:
                //erro de servidor
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);

               //aqui eu estou colocando false = porque neste caso eu quero pegar o erro que vem do servidor.
                case 400:
                    return false;
            }
            //na parte de cima eu faço o tratamento de erro com 401, 403, 404, 500
            //Se acaso chegar aqui ai tem que sucesso 200, 201 ...
            response.EnsureSuccessStatusCode();

            //O meu retorno e (true ou false) olha o meu boleano aguardando lá em cima.
            return true;
        }
    }
}
