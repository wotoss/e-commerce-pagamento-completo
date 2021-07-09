

using Microsoft.EntityFrameworkCore;
using NSE.Catalago.API.Models;
using NSE.Core.Data;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Catalago.API.Data
{
    public class CatalogoContext : DbContext, IUnitOfWork
    {
        //Preciso fazer a configuração na startup dos options para receber a conection String
        public CatalogoContext(DbContextOptions<CatalogoContext> options)
            : base(options) { }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Como boa pratica eu dou este foreach no mapeamento CASO eu escqueça algum campo Mapping ele fará com varcha(100)
            //Isto para evitar o (nvarchar). que seria o default
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
        }

        //Se for acima de zero quer dizer que meu commit deu certo, caso não ele deu errado.
        public async Task<bool> Commit()
        {
            //Lembrando que o base é o meu proprio contexto que seria (IUnitOfWork).
            return await base.SaveChangesAsync() > 0;
        }
    }
}
