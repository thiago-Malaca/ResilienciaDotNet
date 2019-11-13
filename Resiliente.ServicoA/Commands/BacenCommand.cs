using System;
using System.Threading.Tasks;

using Polly;
using Polly.Contrib.WaitAndRetry;

using Resiliente.ServicoA.Entities;
using Resiliente.ServicoA.Services;

using Steeltoe.CircuitBreaker.Hystrix;

namespace Resiliente.ServicoA.Commands
{
    public class BacenCommand : HystrixCommand<Preco>
    {
        private readonly IBacenService _bacenService;
        public BacenCommand(IHystrixCommandOptions options, IBacenService bacenService) : base(options)
        {
            _bacenService = bacenService;
        }

        protected override async Task<Preco> RunAsync()
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(0.5), retryCount: 3);
            var waitAndRetryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(delay);

            return await waitAndRetryPolicy.ExecuteAsync(async() => await _bacenService.GetPrecificacaoAsync());
        }
    }
}