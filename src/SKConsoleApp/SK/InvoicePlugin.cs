using Microsoft.SemanticKernel;
using SemanticKernel.Services;
using SemanticKernel.Domain;
using SemanticKernel.VectorStore;
using System.ComponentModel;

namespace SemanticKernel.SK;

public sealed class InvoicePlugin(InvoiceService invoiceService, AggregationService aggregationService, VectorSearchService vectorSearch)
{
    private readonly InvoiceService _invoiceService = invoiceService;
    private readonly AggregationService _aggregationService = aggregationService;
    private readonly VectorSearchService _vectorSearch = vectorSearch;

    [KernelFunction("obtener_factura")]
    [Description("Obtiene los detalles de una factura específica dado su código.")]
    public Invoice? ObtenerFactura([Description("Código identificador de la factura")]string invoiceCode, CancellationToken ct = default)
    {
        var lst = _invoiceService.GetAll();
        var invoice = lst.FirstOrDefault(i => i.Code.Equals(invoiceCode, StringComparison.OrdinalIgnoreCase));
        return invoice;
    }

    [KernelFunction("filtrar_facturas")]
    [Description("Obtiene las facturas filtradas por diferentes criterios")]
    public async Task<IEnumerable<Invoice>> FiltrarFacturasAsync(string? clienteId = null, DateTime? desde = null, DateTime? hasta = null, string? estado = null, decimal? minTotal = null, decimal? maxTotal = null, CancellationToken ct = default)
    {
        DateTime? from = desde != null ? desde : null;
        DateTime? to = hasta != null ? hasta : null;
        InvoiceStatus? status = estado != null ? Enum.Parse<InvoiceStatus>(estado, true) : null;
        return await _invoiceService.FilterAsync(clienteId, from, to, status, minTotal, maxTotal, ct);
    }

    [KernelFunction("totales_por_mes")]
    public async Task<IEnumerable<object>> TotalesPorMesAsync(int? year = null, CancellationToken ct = default)
    {
        var data = await _aggregationService.TotalsByMonthAsync(year, ct);
        return data.Select(x => new { x.Month, x.Total });
    }

    [KernelFunction("totales_por_ano")]
    public async Task<IEnumerable<object>> TotalesPorAnoAsync(CancellationToken ct = default)
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
