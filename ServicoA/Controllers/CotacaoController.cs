using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ServicoA.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace ServicoA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CotacaoController : ControllerBase
    {
        private readonly ILogger<CotacaoController> _logger;
        private readonly IFeatureManager _featureManager;

        public CotacaoController(ILogger<CotacaoController> logger, IFeatureManager featureManager)
        {
            _logger = logger;
            _featureManager = featureManager;
        }

        [HttpGet]
        public async Task<IEnumerable<Cotacao>> Get()
        {
            if (_featureManager.IsEnabled("FalhaTotal"))
            {
                throw new System.Exception("Erro forçado!");
            }

            var rng = new Random();
            return await Task.FromResult(Enumerable.Range(0, 2).Select(index => new Cotacao
            {
                Data = DateTime.Now.AddDays(-index),
                    Valor = rng.Next(5, 35) * 0.1 + 4.0,
                    Moeda = "Dolar"
            }).ToArray());
        }
    }
}