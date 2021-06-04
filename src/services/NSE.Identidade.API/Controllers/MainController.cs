using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

//Esta é a controller principal ou base para auxiliar as demais controller
namespace NSE.Identidade.API.Controllers
{
    //Quando eu identifico a minha controller como [ApiController] ele me libera os schemas lá na minha controller. E tambem passa a mim liberar swagger o (formato json) e não como formulario
    [ApiController]
    //por ela ser abstract só pode ser herdada e não trabalhada direto.
    public abstract class MainController : Controller
    {
        //estou fazendo um a lista de string onde terei varias msg de erro
        protected ICollection<string> Erros = new List<string>();

        //vou tratar este (customResponse) para responder a um badRequest => Caso haja um erro
        protected ActionResult CustomResponse(object result = null) //este objeto por parametro pode recber (nulo)
        {
            //se a operação for valida
            if (OperacaoValida())
            {
                return Ok(result);
            }
            //caso aconteça um erro
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                //(Mensagens => string) já (Erros.ToArray() => string [])
                //desta forma eu faço a montagem do meu objeto no (return BadRequest)
                {"Mensagens", Erros.ToArray() }
            }));
        }


        //Se existir algun erro dentro desta coleção de erro. significa que a operação não ocorreu com sucesso.
        protected bool OperacaoValida()
        {
             
            return !Erros.Any();
        }

        //agora nós vamos passar estes erro para minha coleção de Erros ! veja os métodos
        protected void AdicionarErroProcessamento(string erro)
        {
            Erros.Add(erro);
        }


        //Erro de validação da ViewModel => procura erros atraves (ModelStateDictionary)
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                AdicionarErroProcessamento(erro.ErrorMessage);
            }
            return CustomResponse();
        }


        //caso precise limpar erro é só chamar o método LimparErrosProcessamento
        protected void LimparErrosProcessamento()
        {
            Erros.Clear();
        }
    }
}
