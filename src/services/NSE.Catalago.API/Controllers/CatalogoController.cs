using Microsoft.AspNetCore.Mvc;
using NSE.Catalago.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Catalago.API.Controllers
{
        [ApiController]
        public class CatalogoController : Controller
        {
            private readonly IProdutoRepository _produtoRepository;

            public CatalogoController(IProdutoRepository produtoRepository)
            {
                _produtoRepository = produtoRepository;
            }
          
        //Aqui eu vou retornar todos os meus produtos
        [HttpGet("catologo/produtos")]
        public async Task<IEnumerable<Produto>> Index()
        {
            return await _produtoRepository.ObterTodos();
        }

        //Aqui vou retornar produto por id
        [HttpGet("catalogo/produtos/{id}")]
        public async Task<Produto> ProdutoDetalhe(Guid id)
        {
            return await _produtoRepository.ObterPorId(id);
        }

    }
}
