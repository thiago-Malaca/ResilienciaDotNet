using System;

namespace Bacen.Entities
{
    public class Cotacao
    {
        public DateTime Data { get; set; }

        public decimal Valor { get; set; }

        public string Moeda { get; set; }
    }
}
