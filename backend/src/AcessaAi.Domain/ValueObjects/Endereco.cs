using AcessaAi.Domain.Avaliacoes.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcessaAi.Domain.ValueObjects
{
    public class Endereco
    {
        public string Logradouro { get; set; } = string.Empty;
        public string UF { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
        public List<Avaliacao> Avaliacoes { get; set; }

    }
}
