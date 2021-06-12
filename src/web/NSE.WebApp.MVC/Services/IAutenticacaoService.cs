using NSE.WebApp.MVC.Models;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    //ESTA INTERFACE IRÁ FAZER A COMUNICAÇÃO COM A API IDENTIFICAÇÃO
    public interface IAutenticacaoService
    {
        //Lembrando que para fazer a comunicação com a (API-REST) temos que trabalhar com métodos asincronos (task-async-await)
        Task<string> Login(UsuarioLogin usuarioLogin);

        Task<string> Registro(UsuarioRegistro usuarioRegistro);
    }

   
}
