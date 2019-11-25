using System.Collections.Generic;
using System.Threading.Tasks;

using Resiliente.ServicoA.Entities;

namespace Resiliente.ServicoA.Services
{
    public interface IBacenService
    {
        Task<Preco> PrecoHoje();

        Task<Preco> PrecoOntem();
    }
}