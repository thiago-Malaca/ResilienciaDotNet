using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Resiliente.ServicoA.Entities;

namespace Resiliente.ServicoA.Services
{
    public class BacenService : IBacenService
    {
        private readonly HttpClient _client;

        public BacenService()
        {
            _client = new HttpClient();
        }

        public async Task<Preco> GetPrecificacaoAsync()
        {
            var lista = await listCotacaoAsync();
            var cotacaoAtual = lista[0];

            var fatorSpred = await fatorSpredAsync();

            return new Preco
            {
                Data = cotacaoAtual.Data,
                Cliente = "Fulano de Tal",
                Valor = cotacaoAtual.Valor * fatorSpred,
                Moeda = cotacaoAtual.Moeda
            };
        }

        private async Task<List<Cotacao>> listCotacaoAsync()
        {
            var response = await _client.GetAsync("http://resiliente_bacen:1501/cotacao");
            if (response == null)
                return null;

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Cotacao>>(jsonString);

            return result;
        }

        private async Task<decimal> fatorSpredAsync()
        {
            return await Task.FromResult((decimal) 1.005);
        }
    }
}