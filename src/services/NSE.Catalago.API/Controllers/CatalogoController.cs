using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Catalago.API.Models;
using NSE.WebAPI.Core.Identidade;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Catalago.API.Controllers
{
        [ApiController]
        [Authorize] //vou trancar minha controller com ([Authorize] )
        //para acessar a minha controler e os metodos, você tem que estar autorizado.
         public class CatalogoController : Controller
          {
            private readonly IProdutoRepository _produtoRepository;

            public CatalogoController(IProdutoRepository produtoRepository)
            {
                _produtoRepository = produtoRepository;
            }
          
        //Eu vou deixar que ele leia os meus produtos
        [AllowAnonymous]
        //Aqui eu vou retornar todos os meus produtos
        [HttpGet("catalogo/produtos")]
        public async Task<IEnumerable<Produto>> Index()
        {
            return await _produtoRepository.ObterTodos();
        }

        //este é o nome do meu método (ClaimsAuthorize)
        //Então eu passo o nome da (Claim=> Catalogo e o valor (Ler))
        //RESUMINDO => para o usuário acessar o método (ProdutoDetalhe)
        //Alem de estar Logado ele tem que ter claim Catalogo com o valor Ler
        [ClaimsAuthorize("Catalogo", "Ler")]
        //Aqui vou retornar produto por id
        [HttpGet("catalogo/produtos/{id}")]
        public async Task<Produto> ProdutoDetalhe(Guid id)
        {
            //erro especializado
            //throw new Exception("Erro!");
            return await _produtoRepository.ObterPorId(id);
        }

    }
}
