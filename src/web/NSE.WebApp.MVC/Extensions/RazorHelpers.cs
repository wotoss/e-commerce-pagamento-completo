using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace NSE.WebApp.MVC.Extensions
{
    //Esta class RazorHelpers é muito bom para evitar codigo na sua viu. Você só chama as ações.
    public static class RazorHelpers
    {
        //Aplicação web onde você registra o seu gravatar para email
        public static string HashEmailForGravatar(this RazorPage page, string email)
        {
            //vou criar um hash MD5
            var md5Hasher = MD5.Create();
            //vou computar este hash com base no email
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(email));
            //atraves do (StringBuilder) eu vou fazer o Build
            var sBuilder = new StringBuilder();
            foreach (var t in data)
            {
                //com a string  formato x2
                sBuilder.Append(t.ToString("x2"));
            }
            //e aqui realmente eu transform ele em uma string
            //fiz tudo isto porque é a forma de conseguir um caminho de imagem de um gravatar especifico.
            return sBuilder.ToString();
        }


        public static string FormatoMoeda(this RazorPage page, decimal valor)
        {
            //Aqui eu estou fazendo a a formatação da minha moeda mas conforme a cultura ou cultura do meu browser
            return valor > 0 ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:C}", valor) : "Gratuito";
        }

        //Mensagem para o cliente sobre o estoque.
        public static string MensagemEstoque(this RazorPage page, int quantidade)
        {
            //Estamos com um ternario
            //Se a (quantidade) for maior do que > 0 que dizer tiver produto passamos ($"Apenas {quantidade} em estoque!")
            //caso seja menor do que zero || ou seja não tenha produto => passamos ("Produto esgotado!")
            return quantidade > 0 ? $"Apenas {quantidade} em estoque!" : "Produto esgotado!";
        }
    }
}
