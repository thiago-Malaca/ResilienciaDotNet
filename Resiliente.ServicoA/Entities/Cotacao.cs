using System;

namespace Resiliente.ServicoA.Entities
{
    public class Cotacao
    {
        public DateTime Data { get; set; }

        public double Valor { get; set; }

        public string Moeda { get; set; }
    }
}