using System.Threading.Tasks;

namespace Bacen.Services
{
    public interface IFlagService
    {
        bool Get(string chave);
        bool Set(string chave, int valor);
    }
}