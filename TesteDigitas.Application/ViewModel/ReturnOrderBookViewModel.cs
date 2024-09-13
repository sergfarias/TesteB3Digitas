using MongoDB.Bson;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteDigitas.Application.ViewModel
{
    public class ReturnOrderBookViewModel
    {
        public ObjectId _id { get; set; }
        public DataViewModel data { get; set; }
        [Column("channel")]
        public string channel { get; set; }
        public string @event { get; set; }
    }
}
