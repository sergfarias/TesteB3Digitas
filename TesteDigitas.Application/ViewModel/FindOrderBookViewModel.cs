namespace TesteDigitas.Application.ViewModel
{
    public class FindOrderBookViewModel
    {
        public string Operation { get; set; }
        public string Channel { get; set; }
        public double BigPreci { get; set; }
        public double MinorPreci { get; set; }
        public double AveragePreci { get; set; }
        public double AveragePreciFiveSeconds { get; set; }
        public double AverageQuantityAccumulate { get; set; }
    }
}
