using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    //Vamos criar os métodos que vão nos fornecer suporte á todas funcionalidade.
    //Lembrete => Uma (ActionResult) sempre precisa retornar algo.
    public class IdentidadeController : Controller
    {       
        private readonly IAutenticacaoService _autenticacaoService;

        //Vou injetar na minha controler este serviço de autenticação
        public IdentidadeController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Registro()
        {

            return View();
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
        {
            //se a modelState não for valida eu retorno a minha view com os dados da minha (model=>usuarioRegistro) = Assim informa o erro na tela
            if (!ModelState.IsValid) return View(usuarioRegistro);
            //Vamos se comunicar com a API - Para fazer o Registro

            var resposta = await _autenticacaoService.Registro(usuarioRegistro);

            //se der errado.
            if (false) return View(usuarioRegistro);

            //Realizar o login na aplicação

            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login (UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return View(usuarioLogin);

            //Se comunicar com a API - Login
            var resposta = await _autenticacaoService.Login(usuarioLogin);

           
            if (false) return View(usuarioLogin);

            //Realizar login na APP

            return RedirectToAction("Index", "Home");

        }

        //No logout ele vai Limpar o Cookie.
        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
