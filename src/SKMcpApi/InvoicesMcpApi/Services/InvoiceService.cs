using InvoicesMcpApi.Domain;

namespace InvoicesMcpApi.Services;

public class InvoiceService
{
    private readonly List<Invoice> _invoices = [];

    public InvoiceService()
    {
        var random = new Random();

        // Clientes de ejemplo
        var customers = new List<(string Id, string Name, string Document)>
        {
            ("CUST001", "Juan Pérez", "12345678A"),
            ("CUST002", "María López", "87654321B"),
            ("CUST003", "Carlos Gómez", "11223344C"),
            ("CUST004", "Ana Torres", "55667788D"),
            ("CUST005", "Luis Martínez", "99887766E"),
            ("CUST006", "Sofía Ramírez", "33445566F"),
            ("CUST007", "Pedro Castillo", "77889900G"),
            ("CUST008", "Lucía Fernández", "44556677H"),
            ("CUST009", "Diego Morales", "22334455I"),
            ("CUST010", "Elena Vargas", "66778899J")
        };

        for (int i = 1; i <= 500; i++)
        {
            var customer = customers[random.Next(customers.Count)];

            var invoice = new Invoice
            {
                Code = $"INV-{i:0000}",
                CustomerId = customer.Id,
                CustomerName = customer.Name,
                CustomerDocument = customer.Document,
                Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-random.Next(0, 365))),
                Status = (InvoiceStatus)random.Next(0, 3),
                Total = Math.Round((decimal)(random.NextDouble() * 1000 + 50), 2)
            };
            
            _invoices.Add(invoice);
        }
    }

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
