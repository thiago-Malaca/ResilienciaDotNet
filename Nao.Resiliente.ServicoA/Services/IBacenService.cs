using System.Collections.Generic;
using System.Threading.Tasks;

using Nao.Resiliente.ServicoA.Entities;

namespace Nao.Resiliente.ServicoA.Services
{
    public interface IBacenService
    {
        Task<Preco> GetPrecificacaoAsync();
    }
}