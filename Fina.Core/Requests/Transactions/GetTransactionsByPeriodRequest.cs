namespace Fina.Core.Requests.Transactions;

/// <summary>
/// Primeiro dia do mes - ultimo dia do m�s.
/// </summary>

public class GetTransactionsByPeriodRequest : PagedRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}