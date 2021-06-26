using System;
using System.Collections.Generic;

namespace NSE.WebApp.MVC.Models
{
    public class ErrorViewModel
    {
      //O meu (ErrorViewModel) terá esta estrutura ou modelo CodigodeErro, titulo e Mensagem
      public int ErroCode { get; set; }

      public string Titulo { get; set; }

      public string Mensagem { get; set; }
    }

    //estou fazendo um tratamento de erro com base do meu retorno do (response ou servidor).
    public class ResponseResult
    {
        public string Title { get; set; }

        public int Status { get; set; }

        public ResponseErrorMessages Errors { get; set; }
    }

    public  class ResponseErrorMessages
    {
        public List<string> Mensagens { get; set; }
    }
}
