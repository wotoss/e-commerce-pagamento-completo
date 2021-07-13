using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public class CatalogoService : Service, ICatalogoService
    {
        //biblioteca (HttpClient) de comunicação WEB com a API => Fazendo isto eu posso conversar fazer request e receber response.
        private readonly HttpClient _httpClient;

        //Inicializo o (HttpClient) pelo construtor 
        public CatalogoService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            //Veja vou setar no meu construtor o enderecoBase = BaseAddress
            //desta forma toda vez que eu api/identidade/autenticar ele já completa com  _settings.AutenticacaoUrl/api/identidade/autenticar 
            httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);

            _httpClient = httpClient;

        }

        public async Task<ProdutoViewModel> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ProdutoViewModel>(response);
        }

        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            var response = await _httpClient.GetAsync("/catalogo/produtos");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<ProdutoViewModel>>(response);
        }
    }
}