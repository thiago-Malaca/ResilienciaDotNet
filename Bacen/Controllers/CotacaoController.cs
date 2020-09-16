using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Bacen.Entities;
using Bacen.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bacen.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CotacaoController : ControllerBase
    {
        private IFlagService _flag;

        public CotacaoController(IFlagService flag)
        {
            _flag = flag;
        }

        [HttpGet]
        public async Task<IEnumerable<Cotacao>> Get()
        {
            if (falhaTotal() || falhaIntermitente())
                return null;

            return await sucesso();
        }

        private bool falhaTotal()
        {
            var erro = _flag.Get("erro-total");
            if (!erro)
                return false;

            var tempo = tempoEmMs();

            Thread.Sleep((int)(tempo % 100) + 250);
            throw new System.Exception("Erro forçado!");
        }

        private bool falhaIntermitente()
        {
            var erro = _flag.Get("erro-intermitente");
            if (!erro)
                return false;

            var tempo = tempoEmMs();

            if ((tempo / 10 % 20) == 0)
            {
                Thread.Sleep((int)(tempo % 100) * 10 + 150);
                throw new System.Exception("Erro de intermitente!");
            }

            return false;
        }

        private async Task<IEnumerable<Cotacao>> sucesso()
        {
            var tempo = tempoEmMs();
            Thread.Sleep((int)(tempo % 50) + 550);
            var rng = new Random();
            return await Task.FromResult(Enumerable.Range(0, 2).Select(index => new Cotacao
            {
                Data = DateTime.Now.AddDays(-index),
                    Valor = (decimal) (rng.Next(5, 35) * 0.01 + 4.0),
                    Moeda = "Dolar"
            }).ToArray());
        }

        private long tempoEmMs()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalMilliseconds;
        }
    }
}