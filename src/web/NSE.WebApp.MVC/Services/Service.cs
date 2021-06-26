using NSE.WebApp.MVC.Extensions;

using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    //Classe abstrata ela só pode ser herdade e não instânciada 
    public abstract  class Service
    {
        //estou passando como paramentro um objeto dado = (object dado)
        protected StringContent ObterConteudo(object dado)
        {
            //aqui no meu login vou fazer uma chamada para o mundo externo.
            //o meu content ou (loginContent) é um conteudo que será enviado então precisa ser (Serializado)
            return new StringContent( //esta instância (new StringContent) vai me retornar um dado no formato String
                JsonSerializer.Serialize(dado), //eu vou serializar no formato Json (então uso JsonSerializer). Estou serializando o (usuarioLogin)
                Encoding.UTF8,
                "application/json"); //Encoding.UTF8 é o formato. Já o (application/json) é o meu (header ou cabeçalho = do json que estou passando)
        }

        //Vamos fazer um método generico quem herdar a classe service, lembrando ela só pode ser herdada pois é abstrata.
        //Então quem herdar a (classe service) herda os métodos.
        protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            //Usando o (PropertyNameCaseInsensitive) eu estou dizendo. Desconsidere maiusculo e minusculo..Neste caso tudo passa a ser igual (A ou a)
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            //Ele vem como um objeto com muita informação. Então vamos deserializar 
            //Passo o meu Deserialize<Com um formato que eu escolher neste caso <string>
            //Este (response.Content) que é meu cotrudo eu passo o formato => (ReadAsStringAsync =>para string)
            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);

        }





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
