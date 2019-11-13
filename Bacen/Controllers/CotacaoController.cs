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

            if ((tempo / 10 % 25) == 0)
            {
                this.HttpContext.Response.StatusCode = 401;
                return true;
            }

            if ((tempo / 10 % 26) == 0)
            {
                this.HttpContext.Response.StatusCode = 403;
                return true;
            }

            if ((tempo / 10 % 100) == 0)
            {
                this.HttpContext.Response.StatusCode = 404;
                return true;
            }

            Thread.Sleep((int) (tempo % 100) * 65);
            throw new System.Exception("Erro forçado!");
        }

        private bool falhaIntermitente()
        {

            if (!_featureManager.IsEnabled("Tem_Falha_Intermitente"))
                return false;

            var tempo = tempoEmMs();

            if ((tempo / 10 % 25) == 0)
            {
                this.HttpContext.Response.StatusCode = 401;
                return true;
            }

            if ((tempo / 10 % 26) == 0)
            {
                this.HttpContext.Response.StatusCode = 403;
                return true;
            }

            if ((tempo / 10 % 10) == 2)
            {
                Thread.Sleep((int) (tempo % 100) * 50);
                throw new System.Exception("Erro de intermitente!");
            }

            return false;
        }

        private async Task<IEnumerable<Cotacao>> sucesso()
        {
            var tempo = tempoEmMs();
            Thread.Sleep((int) (tempo % 100) * 11);
            var rng = new Random();
            return await Task.FromResult(Enumerable.Range(0, 2).Select(index => new Cotacao
            {
                Data = DateTime.Now.AddDays(-index),
                    Valor = (decimal)(rng.Next(5, 35) * 0.01 + 4.0),
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