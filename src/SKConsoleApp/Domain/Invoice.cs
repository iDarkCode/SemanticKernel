using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SemanticKernel.Domain;

public enum InvoiceStatus { Draft, Sent, Paid, Overdue, Cancelled }

public record Invoice
{

    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Description("Codigo unico de factura, identificador alternativo")]
    public string Code { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerDocument { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    
    [JsonPropertyName("Status")]
    [Description("Enum para el estado de la factura, Draft, Sent, Paid, Overdue, Cancelled")]
    public InvoiceStatus Status { get; set; }
    public decimal Total { get; set; }
}
