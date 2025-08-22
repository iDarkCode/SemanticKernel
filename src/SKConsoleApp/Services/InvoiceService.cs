using SemanticKernel.Domain;

namespace SemanticKernel.Services;

public class InvoiceService
{
    private readonly List<Invoice> _invoices = new()
    {
        new Invoice { CustomerId = "C001", Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)), Status = InvoiceStatus.Paid, Total = 1000 },
        new Invoice { CustomerId = "C002", Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-40)), Status = InvoiceStatus.Overdue, Total = 500 },
        new Invoice { CustomerId = "C001", Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-70)), Status = InvoiceStatus.Sent, Total = 750 },
    };

    public IEnumerable<Invoice> GetAll() => _invoices;

    public Task<IEnumerable<Invoice>> FilterAsync(string? customerId, DateOnly? from, DateOnly? to, InvoiceStatus? status, decimal? min, decimal? max, CancellationToken ct)
    {
        var query = _invoices.AsQueryable();
        if (!string.IsNullOrEmpty(customerId)) query = query.Where(x => x.CustomerId == customerId);
        if (from.HasValue) query = query.Where(x => x.Date >= from.Value);
        if (to.HasValue) query = query.Where(x => x.Date <= to.Value);
        if (status.HasValue) query = query.Where(x => x.Status == status.Value);
        if (min.HasValue) query = query.Where(x => x.Total >= min.Value);
        if (max.HasValue) query = query.Where(x => x.Total <= max.Value);
        return Task.FromResult(query.AsEnumerable());
    }
}
