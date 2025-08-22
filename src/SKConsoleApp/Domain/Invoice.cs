namespace SemanticKernel.Domain;

public enum InvoiceStatus { Draft, Sent, Paid, Overdue, Cancelled }

public class Invoice
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CustomerId { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public InvoiceStatus Status { get; set; }
    public decimal Total { get; set; }
}
