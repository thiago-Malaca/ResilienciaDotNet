using System;

namespace Nao.Resiliente.ServicoA.Entities
{
    public class Cotacao
    {
        public DateTime Data { get; set; }

        public decimal Valor { get; set; }

        public string Moeda { get; set; }
    }
}