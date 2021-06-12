
using NSE.WebApp.MVC.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    //Tecnicamente por ter uma interface (IAutenticacaoService) eu preciso fazer tambem á classe (AutenticacaoService)
    public class AutenticacaoService : IAutenticacaoService
    {
        //biblioteca (HttpClient) de comunicação WEB com a API => Fazendo isto eu posso conversar fazer request e receber response.
        private readonly HttpClient _httpClient;

        //Inicializo o (HttpClient) pelo construtor 
        public AutenticacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Login(UsuarioLogin usuarioLogin)
        {
            //aqui no meu login vou fazer uma chamada para o mundo externo.
            //o meu content ou (loginContent) é um conteudo que será enviado então precisa ser (Serializado)
            var loginContent = new StringContent( //esta instância (new StringContent) vai me retornar um dado no formato String
                JsonSerializer.Serialize(usuarioLogin), //eu vou serializar no formato Json (então uso JsonSerializer). Estou serializando o (usuarioLogin)
                Encoding.UTF8, "application/json"); //Encoding.UTF8 é o formato. Já o (application/json) é o meu (header ou cabeçalho = do json que estou passando)

            //Nosso login vai fazer um post nesta url passando este conteudo (loginContent)
            //Simplificando esta url vai receber o meu conteudo (loginContent) - via (POST).
            var response = await _httpClient.PostAsync("https://localhost:44308/api/identidade/autenticar", loginContent);

            var teste = await response.Content.ReadAsStringAsync();

            //Ele vem como um objeto com muita informação. Então vamos deserializar 
            //Passo o meu Deserialize<Com um formato que eu escolher neste caso <string>
            //Este (response.Content) que é meu cotrudo eu passo o formato => (ReadAsStringAsync =>para string)
            return JsonSerializer.Deserialize<string>(await response.Content.ReadAsStringAsync());
        }

        //Este método (Registro) é igual ao de (Login) que já esta comentado para entendimento.
        public async Task<string> Registro(UsuarioRegistro usuarioRegistro)
        {
            var registroContent = new StringContent(
                JsonSerializer.Serialize(usuarioRegistro),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("https://localhost:44308/api/identidade/nova-conta", registroContent);

            return JsonSerializer.Deserialize<string>(await response.Content.ReadAsStringAsync());
        }
    }
}
