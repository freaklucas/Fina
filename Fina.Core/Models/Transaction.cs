using Fina.Core.Enums;

namespace Fina.Core.Models;

/// <summary>
///     Lançamento financeiro...
///     Compras, salário mês...
/// </summary>
/// DataPagamentoOuRecebimento nula -> Struct DATETIME do c# sempre vem preenchida PADRAO
public class Transaction
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? PaidOrReceivedAt { get; set; }
    public decimal Amount { get; set; }
    
    /// <summary>
    ///     Tipo da transação => E||S
    /// </summary>
    public ETransactionType Type { get; set; } = ETransactionType.Withdraw;

    public long CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
}