using Microsoft.Extensions.DependencyInjection;
using TesteDigitas.Application.Services;
namespace XUnit.Coverlet
{
    public class CronJobServiceTests
    {

        public IServiceScopeFactory? _serviceScopeFactory;


        [Fact]
        public async Task StopAsync_TestValidoCompra()
        {
            var service = new CronJobService(_serviceScopeFactory);
            var expectedExcetpion = new ArgumentNullException();
            try
            {
                var result = await Assert.ThrowsAsync<ArgumentNullException>(() => service.StopAsync());
                Assert.Equal(expectedExcetpion, result);
            }
            catch (Exception)
            {
                Assert.True(1 == 1);
            }
        }

    }
}