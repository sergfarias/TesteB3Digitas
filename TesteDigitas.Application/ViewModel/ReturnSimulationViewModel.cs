using System;
using System.Collections.Generic;

namespace TesteDigitas.Application.ViewModel
{
    public class ReturnSimulationViewModel
    {
        public Guid Id { get; set; }
        public List<string> ColecaoUsada { get; set; }
        public decimal Quatidade { get; set; }
        public string Operacao { get; set; }
        public double Resultado { get; set; }
        public string DataCriacao { get; set; }
    }
}
