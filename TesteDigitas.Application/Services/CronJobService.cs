using System;
using System.Threading;
using System.Threading.Tasks;
using TesteDigitas.Application.Settings;
using Microsoft.Extensions.DependencyInjection;
using TesteDigitas.Application.Services.BitStamp;
using TesteDigitas.Application.Interfaces.Enum;
namespace TesteDigitas.Application.Services
{
    public class CronJobService
    {
        public IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;

        public CronJobService(IServiceScopeFactory serviceScopeFactory)=> _serviceScopeFactory = serviceScopeFactory;

        public Task StartAsync()
        {
            Console.WriteLine("CronJob executando");
            var timerProgramado = TimeSpan.FromSeconds(CronJobSettings.Runtime).TotalMilliseconds;
            _timer = new Timer(DoWork, null, 0, Convert.ToInt32(timerProgramado));
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            _timer?.Change(Timeout.Infinite,0);
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using(var scope = _serviceScopeFactory.CreateScope())
            {
                IServiceProvider provider = scope.ServiceProvider;
                var _importador = provider.GetRequiredService<IOrderBookService>();
                try
                {
                    _importador.ReturnData("btcusd");
                    _importador.ReturnData("ethusd");
                    var compra = _importador.Calculate(Operacao.Compra);
                    if (compra != null)
                    {
                        foreach (var item in compra.Result)
                        {
                            if (item != null)
                            {
                                Console.Write("\n");
                                Console.WriteLine("Executando processo:{0}", DateTime.Now + "\n");
                                Console.Write("Operation:" + item.Operation + "\n");
                                Console.Write("Channel:" + item.Channel + "\n");
                                Console.Write("BigPreci:" + item.BigPreci + "\n");
                                Console.Write("MinorPreci:" + item.MinorPreci + "\n");
                                Console.Write("AveragePreci:" + item.AveragePreci + "\n");
                                Console.Write("AveragePreciFiveSeconds:" + item.AveragePreciFiveSeconds + "\n");
                                Console.Write("AverageQuantityAccumulate:" + item.AverageQuantityAccumulate + "\n");
                                Console.Write("\n");
                            }
                        }
                    }
                    var venda = _importador.Calculate(Operacao.Venda);
                    if (venda != null)
                    {
                        foreach (var item in venda.Result)
                        {
                            if (item != null)
                            {
                                Console.Write("\n");
                                Console.WriteLine("Executando processo:{0}", DateTime.Now + "\n");
                                Console.Write("Operation:" + item.Operation + "\n");
                                Console.Write("Channel:" + item.Channel + "\n");
                                Console.Write("BigPreci:" + item.BigPreci + "\n");
                                Console.Write("MinorPreci:" + item.MinorPreci + "\n");
                                Console.Write("AveragePreci:" + item.AveragePreci + "\n");
                                Console.Write("AveragePreciFiveSeconds:" + item.AveragePreciFiveSeconds + "\n");
                                Console.Write("AverageQuantityAccumulate:" + item.AverageQuantityAccumulate + "\n");
                                Console.Write("\n");
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            Console.WriteLine("Processo executado com sucesso!");
        }

    }
}
