using Microsoft.SemanticKernel;
using SemanticKernel.Services;
using SemanticKernel.Domain;
using SemanticKernel.VectorStore;

namespace SemanticKernel.SK;

public sealed class InvoicePlugin
{
    private readonly InvoiceService _invoiceService;
    private readonly AggregationService _aggregationService;
    private readonly VectorSearchService _vectorSearch;

    public InvoicePlugin(InvoiceService invoiceService, AggregationService aggregationService, VectorSearchService vectorSearch)
    {
        _invoiceService = invoiceService;
        _aggregationService = aggregationService;
        _vectorSearch = vectorSearch;
    }

    [KernelFunction("filtrar_facturas")]
    public async Task<IEnumerable<Invoice>> FiltrarFacturasAsync(string? clienteId = null, string? desde = null, string? hasta = null, string? estado = null, decimal? minTotal = null, decimal? maxTotal = null, CancellationToken ct = default)
    {
        DateOnly? from = desde != null ? DateOnly.Parse(desde) : null;
        DateOnly? to = hasta != null ? DateOnly.Parse(hasta) : null;
        InvoiceStatus? status = estado != null ? Enum.Parse<InvoiceStatus>(estado, true) : null;
        return await _invoiceService.FilterAsync(clienteId, from, to, status, minTotal, maxTotal, ct);
    }

    [KernelFunction("totales_por_mes")]
    public async Task<IEnumerable<object>> TotalesPorMesAsync(int? year = null, CancellationToken ct = default)
    {
        var data = await _aggregationService.TotalsByMonthAsync(year, ct);
        return data.Select(x => new { x.Month, x.Total });
    }

    [KernelFunction("totales_por_año")]
    public async Task<IEnumerable<object>> TotalesPorAñoAsync(CancellationToken ct = default)
    {
        var data = await _aggregationService.TotalsByYearAsync(ct);
        return data.Select(x => new { x.Year, x.Total });
    }

    [KernelFunction("buscar_semantico")]
    public async Task<IEnumerable<object>> BuscarSemanticoAsync(string query, int topK = 5, CancellationToken ct = default)
    {
        var results = await _vectorSearch.SearchAsync(query, topK, ct);
        return results.Select(r => new { r.Invoice.Id, r.Invoice.CustomerId, r.Invoice.Total, r.Invoice.Date, Score = r.Score });
    }
}
