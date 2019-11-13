using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

using Resiliente.ServicoA.Entities;
using Resiliente.ServicoA.Services;

namespace Resiliente.ServicoA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrecoController : ControllerBase
    {
        private readonly ILogger<PrecoController> _logger;
        private readonly IFeatureManager _featureManager;

        private readonly IBacenService _bacenService;

        public PrecoController(ILogger<PrecoController> logger, IFeatureManager featureManager, IBacenService bacenService)
        {
            _logger = logger;
            _featureManager = featureManager;
            _bacenService = bacenService;
        }

        [HttpGet]
        public async Task<Preco> Get()
        {
            return await _bacenService.GetPrecificacaoAsync();
        }
    }
}