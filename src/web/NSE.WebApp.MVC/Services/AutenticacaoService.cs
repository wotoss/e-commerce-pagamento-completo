
using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    //Tecnicamente por ter uma interface (IAutenticacaoService) eu preciso fazer tambem á classe (AutenticacaoService)
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        //biblioteca (HttpClient) de comunicação WEB com a API => Fazendo isto eu posso conversar fazer request e receber response.
        private readonly HttpClient _httpClient;

        

        //Inicializo o (HttpClient) pelo construtor 
        public AutenticacaoService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            //Veja vou setar no meu construtor o enderecoBase = BaseAddress
            //desta forma toda vez que eu api/identidade/autenticar ele já completa com  _settings.AutenticacaoUrl/api/identidade/autenticar 
            httpClient.BaseAddress = new Uri(settings.Value.AutenticacaoUrl);

            _httpClient = httpClient;
            
        }
        //APENAS COMO LEMBRETE: ESTE (UsuarioRespostaLogin). Trás o retorno do (token) => foi contruida uma classe para isto
        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
        {
            //Entrar (f12) dentro do método ObterConteudo para entender toda a construção deste método
            var loginContent = ObterConteudo(usuarioLogin);

            //Nosso login vai fazer um post nesta url passando este conteudo (loginContent)44308
            //Simplificando esta url vai receber o meu conteudo (loginContent) - via (POST).44396, 44330
            //OLHA DESTA FORMA EU PASSO PELA MINHA (CLASSE AppSettings) E PEGAR O VALOR DENTRO DO MEU (ARQUIVO appSettings.Development.json || appsettings)
            var response = await _httpClient.PostAsync("api/identidade/autenticar", loginContent);

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    //É importante entrar (f12) no método  DeserializarObjetoResponse para ver como foi implementado.
                   ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }
            //É importante entrar (f12) no método  DeserializarObjetoResponse para ver como foi implementado.
            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }
        //APENAS COMO LEMBRETE: ESTE (UsuarioRespostaLogin). Trás o retorno do (token) => foi contruida uma classe para isto
        //Este método (Registro) é igual ao de (Login) que já esta comentado para entendimento.
        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
        {

            //Entrar (f12) dentro do método ObterConteudo para entender toda a construção deste método
            var registroContent = ObterConteudo(usuarioRegistro);

            //1 = endpoints endereço {_settings.AutenticacaoUrl} a parte do servidor. (esta parte dinâmica pode ser mudada na hospedagem ou em algu momento)
            //2 = endpoinst endereço /api/identidade/nova-conta" a parte de nossa api. (Do nosso Sistema)
            var response = await _httpClient.PostAsync("/api/identidade/nova-conta", registroContent);


            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    //É importante entrar (f12) no método  DeserializarObjetoResponse para ver como foi implementado.
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);

        }
    }
}
