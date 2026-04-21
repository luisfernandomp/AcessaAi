using System;
using System.Collections.Generic;
using System.Text;

namespace AcessaAi.Application.Autenticacao.Dtos
{
    public class LoginResponse
    {
        public int IdUsuario { get; set; }  
        public string NomeUsuario { get; set; }
        public string Token { get; set; }
        public int ExpiraEm { get; set; }
    }
}
