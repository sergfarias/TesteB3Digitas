using System.ComponentModel;

namespace TesteDigitas.Application.Interfaces.Enum
{
    /// <summary>
    /// Enumerador Objeto de transferência dos dados
    /// </summary>
    public enum Operacao
    {
        [Description("Compra")]
        Compra = 0,

        [Description("Venda")]
        Venda = 1

    }
}