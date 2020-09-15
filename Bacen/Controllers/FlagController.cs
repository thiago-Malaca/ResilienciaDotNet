using System.Threading.Tasks;
using Bacen.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bacen.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlagController : ControllerBase
    {
        private IFlagService _flag;

        public FlagController(IFlagService flag)
        {
            _flag = flag;
        }

        [HttpGet("{chave}")]
        public bool Get(string chave)
        {
            return _flag.Get(chave);
        }

        [HttpGet("{chave}/{valor:int}")]
        public bool Get(string chave, int valor)
        {
            return _flag.Set(chave, valor);
        }
    }
}