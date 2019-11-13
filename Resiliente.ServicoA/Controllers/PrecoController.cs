using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Resiliente.ServicoA.Commands;
using Resiliente.ServicoA.Entities;

namespace Resiliente.ServicoA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrecoController : ControllerBase
    {
        private readonly BacenCommand _bacenCommand;

        public PrecoController(BacenCommand bacenCommand)
        {
            _bacenCommand = bacenCommand;
        }

        [HttpGet]
        public async Task<Preco> Get()
        {
            return await _bacenCommand.ExecuteAsync();
        }
    }
}