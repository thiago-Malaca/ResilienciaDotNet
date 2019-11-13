using System;

namespace Resiliente.ServicoA.Entities
{
    public class Preco
    {
        public DateTime Data { get; set; }

        public decimal Valor { get; set; }

        public string Moeda { get; set; }

        public string Cliente { get; set; }
    }
}