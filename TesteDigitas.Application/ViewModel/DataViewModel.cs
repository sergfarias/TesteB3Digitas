using System.Collections.Generic;
namespace TesteDigitas.Application.ViewModel
{
    public class DataViewModel
    {
        public string timestamp { get; set; }
        public string microtimestamp { get; set; }

        public List<string[]> bids { get; set; }

        public List<string[]> asks { get; set; }
    }
}
