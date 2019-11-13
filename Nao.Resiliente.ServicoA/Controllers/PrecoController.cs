using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

using Nao.Resiliente.ServicoA.Entities;
using Nao.Resiliente.ServicoA.Services;

namespace Nao.Resiliente.ServicoA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrecoController : ControllerBase
    {
        private readonly IBacenService _bacenService;

        public PrecoController(IBacenService bacenService)
        {
            _bacenService = bacenService;
        }

        [HttpGet]
        public async Task<Preco> Get()
        {
            return await _bacenService.GetPrecificacaoAsync();
        }
    }
}