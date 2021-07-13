using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
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
}
