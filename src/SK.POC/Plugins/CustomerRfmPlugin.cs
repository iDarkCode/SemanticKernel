using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text.Json;

namespace SK.Course.Plugins;

public class CustomerRfmPlugin()
{
    private readonly List<CustomerRFMDto> _customers = JsonSerializer.Deserialize<List<CustomerRFMDto>>(
            File.ReadAllText("./Resources/customers.json")) ?? [];


    [KernelFunction]
    [Description("Obtiene los datos de un cliente en una sesión concreta por fecha")]
    public CustomerRFMDto? GetCustomerBySession(
        string numCliente, 
        DateTime sesion)
    {
        return _customers.FirstOrDefault(c =>
            c.NumCliente == numCliente && 
            c.Sesion?.Date == sesion.Date);
    }

    [KernelFunction]
    [Description("Obtiene los datos de un cliente en un rango de fechas")]
    public IEnumerable<CustomerRFMDto> GetCustomerByDateRange(
        string numCliente, 
        DateTime start, 
        DateTime end)
    {
        return _customers.Where(c =>
            c.NumCliente == numCliente &&
            c.Sesion >= start && c.Sesion <= end);
    }

    [KernelFunction]
    [Description("Obtiene clientes de una sala en una sesión concreta")]
    public IEnumerable<CustomerRFMDto> GetCustomersByStoreSession(
        int idCentro, 
        DateTime sesion)
    {
        return _customers.Where(c =>
            c.IdCentro == idCentro && c.Sesion?.Date == sesion.Date);
    }

    [KernelFunction]
    [Description("Obtiene clientes de una sala en un rango de fechas")]
    public IEnumerable<CustomerRFMDto> GetCustomersByStoreDateRange(
        int idCentro, 
        DateTime start, 
        DateTime end)
    {
        return _customers.Where(c =>
            c.IdCentro == idCentro && 
            c.Sesion >= start && 
            c.Sesion <= end);
    }   

    [KernelFunction]
    [Description("Obtiene el cliente con mayor saldo en una sala (cliente King)")]
    public CustomerRFMDto? GetKingCustomerByStore(
        int idCentro)
    {
        return _customers
            .Where(c => c.IdCentro == idCentro)
            .OrderByDescending(c => c.Saldo ?? 0)
            .FirstOrDefault();
    }

    [KernelFunction]
    [Description("Obtiene clientes filtrados por KPIs y condiciones avanzadas")]
    public IEnumerable<CustomerRFMDto> GetCustomersByKpis(CustomerKpiFilterRequest filtros)
    {
        var query = _customers.AsQueryable();
        query = CustomerKpiFilterBuilder.ApplyFilters(query, filtros);
        return [.. query];
    }
}

#region AggregateService
public class CustomerKpiFilterRequest
{
    public int? MinFrecuencia { get; set; }
    public int? MaxFrecuencia { get; set; }
    public int? MinRecencia { get; set; }
    public int? MaxRecencia { get; set; }
    public int? MinVisitasMes { get; set; }
    public int? MaxVisitasMes { get; set; }
    public int? MinDiasSinVenir { get; set; }
    public int? MaxDiasSinVenir { get; set; }

    public decimal? MinSaldo { get; set; }
    public decimal? MaxSaldo { get; set; }
    public decimal? MinWinMensual { get; set; }
    public decimal? MaxWinMensual { get; set; }
    public int? MinNivelCliente { get; set; }
    public int? MaxNivelCliente { get; set; }

    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
   
    public int? IdCentro { get; set; }
    public DateTime? Sesion { get; set; }

    public bool? Comunicado { get; set; }
    public bool? Sportium { get; set; }
}

public static class CustomerKpiFilterBuilder
{
    public static IQueryable<CustomerRFMDto> ApplyFilters(
        IQueryable<CustomerRFMDto> query,
        CustomerKpiFilterRequest filters)
    {
    
        if (filters.MinFrecuencia.HasValue)
            query = query.Where(c => (c.Frecuencia ?? 0) >= filters.MinFrecuencia.Value);

        if (filters.MaxFrecuencia.HasValue)
            query = query.Where(c => (c.Frecuencia ?? 0) <= filters.MaxFrecuencia.Value);

        if (filters.MinRecencia.HasValue)
            query = query.Where(c => (c.Recencia ?? int.MaxValue) >= filters.MinRecencia.Value);

        if (filters.MaxRecencia.HasValue)
            query = query.Where(c => (c.Recencia ?? 0) <= filters.MaxRecencia.Value);

        if (filters.MinVisitasMes.HasValue)
            query = query.Where(c => (c.VisitasMesActual ?? 0) >= filters.MinVisitasMes.Value);

        if (filters.MaxVisitasMes.HasValue)
            query = query.Where(c => (c.VisitasMesActual ?? 0) <= filters.MaxVisitasMes.Value);

        if (filters.MinDiasSinVenir.HasValue)
            query = query.Where(c => (c.DiasSinVenir ?? int.MaxValue) >= filters.MinDiasSinVenir.Value);

        if (filters.MaxDiasSinVenir.HasValue)
            query = query.Where(c => (c.DiasSinVenir ?? 0) <= filters.MaxDiasSinVenir.Value);

       
        if (filters.MinSaldo.HasValue)
            query = query.Where(c => (c.Saldo ?? 0) >= filters.MinSaldo.Value);

        if (filters.MaxSaldo.HasValue)
            query = query.Where(c => (c.Saldo ?? decimal.MaxValue) <= filters.MaxSaldo.Value);

        if (filters.MinWinMensual.HasValue)
            query = query.Where(c => (c.WinMensual ?? 0) >= filters.MinWinMensual.Value);

        if (filters.MaxWinMensual.HasValue)
            query = query.Where(c => (c.WinMensual ?? decimal.MaxValue) <= filters.MaxWinMensual.Value);

        if (filters.MinNivelCliente.HasValue)
            query = query.Where(c => (c.NivelCliente ?? 0) >= filters.MinNivelCliente.Value);

        if (filters.MaxNivelCliente.HasValue)
            query = query.Where(c => (c.NivelCliente ?? int.MaxValue) <= filters.MaxNivelCliente.Value);

       
        if (filters.FechaDesde.HasValue)
            query = query.Where(c => (c.FechaUltimaVisita ?? DateTime.MinValue) >= filters.FechaDesde.Value);

        if (filters.FechaHasta.HasValue)
            query = query.Where(c => (c.FechaUltimaVisita ?? DateTime.MaxValue) <= filters.FechaHasta.Value);
             

        if (filters.IdCentro.HasValue)
            query = query.Where(c => c.IdCentroPrincipal == filters.IdCentro);

        if (filters.Sesion.HasValue)
            query = query.Where(c => c.Sesion == filters.Sesion);

     
        if (filters.Comunicado.HasValue)
            query = query.Where(c => c.Comunicado == filters.Comunicado.Value);

        if (filters.Sportium.HasValue)
            query = query.Where(c => c.Sportium == filters.Sportium.Value);

        return query;
    }
}
#endregion
