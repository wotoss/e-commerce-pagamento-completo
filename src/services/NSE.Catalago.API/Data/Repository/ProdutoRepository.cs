using Microsoft.EntityFrameworkCore;
using NSE.Catalago.API.Models;
using NSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Catalago.API.Data.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        //vamos injetar o nosso catalogo contexto via Injeção de dependencia
        private readonly CatalogoContext _context;

        public ProdutoRepository(CatalogoContext context)
        {
            _context = context;
        }
        public IUnitOfWork IUnitOfWork => _context;

        //COLOQUEI MAS PRECISO REAVALIAR
        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public async Task<IEnumerable<Produto>> ObterTodos()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }

        public async Task<Produto> ObterPorId(Guid id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public void Adicionar(Produto produto)
        {
            _context.Produtos.Add(produto);
        }


        public void Atualizar(Produto produto)
        {
            _context.Produtos.Update(produto);
        }

       
        public void Dispose()
        {
            _context?.Dispose();
        }

    }
}
