using SemanticKernel.Domain;

namespace SemanticKernel.Services;

public class AggregationService
{
    private readonly InvoiceService _invoiceService;

    public AggregationService(InvoiceService invoiceService) => _invoiceService = invoiceService;

    public Task<IEnumerable<(int Month, decimal Total)>> TotalsByMonthAsync(int? year, CancellationToken ct)
    {
        var data = _invoiceService.GetAll()
            .Where(i => !year.HasValue || i.Date.Year == year)
            .GroupBy(i => i.Date.Month)
            .Select(g => (g.Key, g.Sum(x => x.Total)));
        return Task.FromResult(data);
    }

    public Task<IEnumerable<(int Year, decimal Total)>> TotalsByYearAsync(CancellationToken ct)
    {
        var data = _invoiceService.GetAll()
            .GroupBy(i => i.Date.Year)
            .Select(g => (g.Key, g.Sum(x => x.Total)));
        return Task.FromResult(data);
    }
}
