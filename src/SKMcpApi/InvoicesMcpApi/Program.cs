using InvoicesMcpApi.Domain;
using SemanticKernel.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<InvoiceService>();
var app = builder.Build();

// Manifest MCP
app.MapGet("/.well-known/ai-plugin.json", () => Results.Json(new
{
    name = "invoices-service",
    version = "1.0.0",
    description = "Servicio para consultar el estado de facturas y realizar búsquedas avanzadas por cliente y rango de fechas",
    tools = new object[]
    {
        new {
            name = "GetInvoiceStatus",
            description = "Obtiene el estado de una factura específica dado su número. Devuelve información como estado, cliente y fecha de emisión.",
            input_schema = new {
                type = "object",
                properties = new {
                    invoiceId = new { type = "string", description = "Número de la factura, por ejemplo '2024-INV-124'" }
                },
                required = new [] { "invoiceId" }
            },
            examples = new []
            {
                new {
                    input = new { invoiceId = "2024-INV-124" },
                    output = new { InvoiceId = "2024-INV-124", CustomerName = "Alice", Status = "Pagada", Date = "2024-01-15" }
                }
            },
            tags = new [] { "facturas", "consulta", "estado" }
        },
        new {
            name = "SearchInvoices",
            description = "Busca facturas filtrando por cliente, rango de fechas, estado y montos.",
            input_schema = new {
                type = "object",
                properties = new {
                    customerId = new { type = "string", description = "Identificador del cliente, por ejemplo 'CUST001'" },
                    from = new { type = "string", format = "date", description = "Fecha inicial del rango (opcional)" },
                    to = new { type = "string", format = "date", description = "Fecha final del rango (opcional)" },
                    status = new { type = "string", description = "Estado de la factura: Pending, Paid o Canceled" },
                    min = new { type = "number", description = "Monto mínimo de la factura (opcional)" },
                    max = new { type = "number", description = "Monto máximo de la factura (opcional)" }
                },
                required = new [] { "customerId" }
            },
            examples = new []
            {
                new {
                    input = new { customerId = "CUST001", from = "2024-01-01", to = "2024-03-31", status = "Paid", min = 100, max = 500 },
                    output = new [] {
                        new { InvoiceId = "2024-INV-124", Date = "2024-01-15", Status = "Paid", Total = 250 },
                        new { InvoiceId = "2024-INV-126", Date = "2024-02-20", Status = "Paid", Total = 300 }
                    }
                }
            },
            tags = new [] { "facturas", "busqueda", "cliente", "rango-fechas", "estado", "monto" }
        }
    }
}));



app.MapPost("/tools/GetInvoiceStatus", async (HttpContext ctx, InvoiceService service) =>
{
    var req = await JsonSerializer.DeserializeAsync<InvoiceRequest>(ctx.Request.Body);
    var invoice = service.GetAll().FirstOrDefault(i => i.Id == req?.InvoiceId);
    var result = invoice is null
        ? $"No se encontró la factura {req?.InvoiceId}"
        : $"La factura {invoice.Id} está en estado: {invoice.Status}";
    await ctx.Response.WriteAsJsonAsync(new { result });
});

app.MapPost("/tools/SearchInvoices", async (HttpContext ctx, InvoiceService service) =>
{
    var req = await JsonSerializer.DeserializeAsync<InvoiceSearchRequest>(ctx.Request.Body);

    var results = await service.FilterAsync(
        customerId: req?.customerId,
        from: req?.from,
        to: req?.to,
        status: req?.status,
        min: req?.min,
        max: req?.max,
        CancellationToken.None
    );

    var output = results.Any()
        ? results.ToList<object>()
        : [$"No se encontraron facturas para {req?.customerId}"];

    await ctx.Response.WriteAsJsonAsync(new { result = output });
});

app.MapPost("/tools/status", async (HttpContext ctx) =>
{
    await ctx.Response.WriteAsJsonAsync(new { result = true });
});

app.Run();

record InvoiceRequest(string InvoiceId);
record InvoiceSearchRequest(string? customerId, DateOnly? from, DateOnly? to, InvoiceStatus? status, decimal? min, decimal? max);//(string CustomerName, string? FromDate, string? ToDate);
