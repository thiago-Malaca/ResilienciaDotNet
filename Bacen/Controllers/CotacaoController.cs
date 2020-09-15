using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Bacen.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace Bacen.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CotacaoController : ControllerBase
    {
        private readonly IFeatureManager _featureManager;

        public CotacaoController(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
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
            if (!_featureManager.IsEnabled("Tem_Falha_Total"))
                return false;

            var tempo = tempoEmMs();

            Thread.Sleep((int) (tempo % 100) + 2500);
            throw new System.Exception("Erro forçado!");
        }

        private bool falhaIntermitente()
        {
            if (!_featureManager.IsEnabled("Tem_Falha_Intermitente"))
                return false;

            var tempo = tempoEmMs();

            if ((tempo / 10 % 20) == 0)
            {
                 Thread.Sleep((int) (tempo % 100) * 10 + 150);
                throw new System.Exception("Erro de intermitente!");
            }

            return false;
        }

        private async Task<IEnumerable<Cotacao>> sucesso()
        {
            var tempo = tempoEmMs();
            Thread.Sleep((int) (tempo % 50) + 550);
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
            return (long) timeSpan.TotalMilliseconds;
        }
    }
}