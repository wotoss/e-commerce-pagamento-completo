using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Services;
using System;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class CatalogoController : MainController//ICatalogoServiceRefit
    {
        private readonly ICatalogoService _catalogoService;
        //Fazendo teste com Refit
        //private readonly ICatalogoServiceRefit _catalogoService;

        public CatalogoController(ICatalogoService catalogoService /*ICatalogoServiceRefit catalogoService*/)
        {
            _catalogoService = catalogoService;
        }


        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index()
        {
            var produtos = await _catalogoService.ObterTodos();
            return View(produtos);
        }

        [HttpGet]
        [Route("produto-detalhe/{id}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            var produto = await _catalogoService.ObterPorId(id);
            return View(produto);
        }
    }
}
