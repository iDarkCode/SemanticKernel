using SemanticKernel.Domain;
using SemanticKernel.Services;

namespace SemanticKernel.VectorStore;

public class VectorSearchService(InvoiceService invoiceService)
{
    private readonly InvoiceService _invoiceService = invoiceService;

    public Task<IEnumerable<(Invoice Invoice, double Score)>> SearchAsync(string query, int topK, CancellationToken ct)
    {
        // Simulación: devuelve facturas con puntaje aleatorio
        var rnd = new Random();
        var results = _invoiceService.GetAll()
            .Select(i => (i, rnd.NextDouble()))
            .OrderByDescending(x => x.Item2)
            .Take(topK);
        return Task.FromResult(results.Select(r => (r.i, r.Item2)));
    }
}
