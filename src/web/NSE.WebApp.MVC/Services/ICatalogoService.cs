using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public interface ICatalogoService
    {
        //Este método (ObterTodos) retorna uma lista (IEnumerable) de (ProdutoViewModel)
        Task<IEnumerable<ProdutoViewModel>> ObterTodos();

        //Já este método (ObterPorId) retorna um (ProdutoViewModel)
        Task<ProdutoViewModel> ObterPorId(Guid id);
    }

    //Vamos usar o Refit => inclusive com as mesmas  assinaturas de métodos
    //Refit é uma forma mais simple de se comunicar eu posso fazer Get, Post, Delete entre outros
    //com o refit eu não preciso usar o (service), fica mais simples.
    public interface ICatalogoServiceRefit
    {
        [Get("/catalogo/produtos")]
        Task<IEnumerable<ProdutoViewModel>> ObterTodos();

        [Get("/catalogo/produtos/{id}")]
        Task<ProdutoViewModel> ObterPorId(Guid id);
    }
}
