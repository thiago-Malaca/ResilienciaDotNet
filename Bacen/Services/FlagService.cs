using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using Bacen.Extensions;


namespace Bacen.Services
{
    public class FlagService : IFlagService
    {
        private IDatabase _cache;

        public FlagService(IConnectionMultiplexer  conn)
        {
            _cache = conn.GetDatabase();
        }
        public Get(string chave)
        {
            var valor = _cache.StringGet(chave).ToString();
            return valor.ParseInt() > 0;
        }

        public Set(string chave, int valor)
        {
            return _cache.StringSet(chave, valor);
        }
    }
}