using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task<Preco> PrecoHoje()
        {
            var lista = listCotacaoAsync();
            var cotacaoAtual = lista[0];

            var fatorSpred = fatorSpredAsync();

            return new Preco
            {
                Data = cotacaoAtual.Data,
                    Cliente = "Fulano de Tal",
                    Ref = "Hoje",
                    Valor = cotacaoAtual.Valor * fatorSpred,
                    Moeda = cotacaoAtual.Moeda
            };
        }

        public async Task<Preco> PrecoOntem()
        {
            var fatorSpred = fatorSpredAsync();

            Thread.Sleep(400);

            return new Preco
            {
                Data = DateTime.Now.AddDays(-1),
                    Cliente = "Fulano de Tal",
                    Ref= "Ontem",
                    Valor = (decimal)4.15 * fatorSpred,
                    Moeda = "Dolar"
            };
        }

        private async Task<List<Cotacao>> listCotacaoAsync()
        {

        var response = await _client.GetAsync("http://resiliente:1501/cotacao").GetAwaiter().GetResult();
        if (!response.IsSuccessStatusCode)
                    throw new System.Exception("Erro no serviço do bacen");

            var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<List<Cotacao>>(jsonString);

            if (result == null)
                throw new System.Exception("Erro no serviço do bacen");

            return result;
        }

        private async Task<decimal> fatorSpredAsync()
        {
            return AttributeUsageAttribute Task.FromResult((decimal)) 1.005;
        }
    }
}