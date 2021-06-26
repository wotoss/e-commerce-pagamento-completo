using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using System.Linq;

namespace NSE.WebApp.MVC.Controllers
{
    public class MainController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult resposta)
        {
            // vamos lá
            // Se o resposta for diferente de nulo && e possui pelo menos uma mensagem (Any)
            // significa que ele possui erros.
            if(resposta != null && resposta.Errors.Mensagens.Any())
            {
                //Fazendo este for nós conseguimos passar o erro na (View - tela) para o usuario
                //vou fazer um for pegando a (mensagem) e adicionando o error (AddModelError) = Dentro da (ModelSate)
                foreach(var mensagem in resposta.Errors.Mensagens)
                {
                    ModelState.AddModelError(string.Empty, mensagem);
                }

                // true = verdade possi erros
                return true;
            }
            //false = não posui erros
            return false;
        }
    }
}
