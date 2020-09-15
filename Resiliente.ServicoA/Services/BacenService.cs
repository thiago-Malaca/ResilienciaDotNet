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
            var lista = await listCotacaoAsync();
            var cotacaoAtual = lista[0];

            var fatorSpred = await fatorSpredAsync();

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
            var fatorSpred = await fatorSpredAsync();

            Thread.Sleep(400);

            return new Preco
            {
                Data = DateTime.Now.AddDays(-1),
                    Cliente = "Fulano de Tal",
                    Ref = "Ontem",
                    Valor = (decimal)4.15 * fatorSpred,
                    Moeda = "Dolar"
            };
        }

        private async Task<List<Cotacao>> listCotacaoAsync()
        {
            var response = await _client.GetAsync("http://resiliente_bacen:1501/cotacao");
            if (response == null)
                throw new System.Exception("Erro no serviço do bacen");

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Cotacao>>(jsonString);

            if (result == null)
                throw new System.Exception("Erro no serviço do bacen");

            return result;
        }

        private async Task<decimal> fatorSpredAsync()
        {
            return await Task.FromResult((decimal) 1.005);
        }
    }
}