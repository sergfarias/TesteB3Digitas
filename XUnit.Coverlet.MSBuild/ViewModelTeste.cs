using TesteDigitas.Application.Settings;
using TesteDigitas.Application.ViewModel;
using static TesteDigitas.Application.ViewModel.ErrorResponseViewModel;

namespace XUnit.Coverlet
{
    public class ViewModelTeste
    {

        [Fact]
        public void ErrorResponseViewModel()
        {
            var service = new ErrorResponseViewModel
            {
                TraceId = "teste"
            };
            Assert.True(service.TraceId=="teste");
        }

        [Fact]
        public void ErrorResponseViewModel1()
        {
            var service = new ErrorResponseViewModel("teste1", "testando")
            {
                TraceId = "teste"
            };
            Assert.True(service.TraceId == "teste");
        }

        [Fact]
        public void ErrorDetailsViewModel()
        {
            var service = new ErrorDetailsViewModel("teste", "testando")
            {
                Logref = "teste"
            };
            Assert.True(service.Logref == "teste");
        }

       
    }
}
