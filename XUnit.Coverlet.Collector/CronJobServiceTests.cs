using Microsoft.Extensions.DependencyInjection;
using TesteDigitas.Application.Services;
namespace XUnit.Coverlet
{
    public class CronJobServiceTests
    {

        public IServiceScopeFactory? _serviceScopeFactory;


       // public CronJobService(IServiceScopeFactory serviceScopeFactory) { _serviceScopeFactory = serviceScopeFactory; }
           

        //[Fact]
        //public async Task StartAsync_TestValidoCompra()
        //{
        //    var service = new CronJobService(_serviceScopeFactory);

        //    await service.StartAsync();

        //    Assert.True(1==1);
        //}

        [Fact]
        public async Task StopAsync_TestValidoCompra()
        {
            var service = new CronJobService(_serviceScopeFactory);

            await service.StopAsync();

            Assert.True(1 == 1);
        }

      

    }
}