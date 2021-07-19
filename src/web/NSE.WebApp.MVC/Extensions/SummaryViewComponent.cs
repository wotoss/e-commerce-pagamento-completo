using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Extensions
{
  //Veja que a minha classe (SummaryViewComponent) herda funcionalidades de (ViewComponent) que é uma classe do AspNet Core
    public class SummaryViewComponent : ViewComponent
    {
        //Veja que simplesmente neste caso eu vou retornar uma View
        public  async Task<IViewComponentResult> InvokeAsync()
        {
            return  View();
        }
    }
}
