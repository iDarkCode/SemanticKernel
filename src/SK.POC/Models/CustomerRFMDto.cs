using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Data;
using Newtonsoft.Json;

public class CustomerRFMDto
{
    [JsonProperty("idCentro")]
    [VectorStoreData]
    public int? IdCentro { get; set; }

    [JsonProperty("fechaCumpleActual")]
    [VectorStoreData]
    public DateTime? FechaCumpleActual { get; set; }

    [JsonProperty("sesion")]
    [VectorStoreData]
    public DateTime? Sesion { get; set; }

    [JsonProperty("nsalas")]
    [VectorStoreData]
    public int? Nsalas { get; set; }

    [JsonProperty("idRecency")]
    [VectorStoreData]
    public string IdRecency { get; set; } = default!;

    [JsonProperty("recenciaMedia")]
    [VectorStoreData]
    public decimal? RecenciaMedia { get; set; }

    [JsonProperty("recencia")]
    [VectorStoreData]
    public int? Recencia { get; set; }

    [JsonProperty("numCliente")]
    [VectorStoreKey] // clave en el vector store
    [TextSearchResultName]
    public string NumCliente { get; set; } = default!;

    [JsonProperty("idMonetaryAnual")]
    [VectorStoreData]
    public int? IdMonetaryAnual { get; set; }

    [JsonProperty("idMonetaryAnualDetallado")]
    [VectorStoreData]
    public int? IdMonetaryAnualDetallado { get; set; }

    [JsonProperty("idMonetary")]
    [VectorStoreData]
    public int? IdMonetary { get; set; }

    [JsonProperty("idFrequencyAnual")]
    [VectorStoreData]
    public int? IdFrequencyAnual { get; set; }

    [JsonProperty("idFrequency")]
    [VectorStoreData]
    public int? IdFrequency { get; set; }

    [JsonProperty("nivelCliente")]
    [VectorStoreData]
    public int? NivelCliente { get; set; }

    [JsonProperty("clubCliente")]
    [VectorStoreData]
    public long? ClubCliente { get; set; }

    [JsonProperty("fechaAltaClub")]
    [VectorStoreData]
    public DateTime? FechaAltaClub { get; set; }

    [JsonProperty("frecuenciaMedia")]
    [VectorStoreData]
    public decimal? FrecuenciaMedia { get; set; }

    [JsonProperty("frecuencia")]
    [VectorStoreData]
    public int? Frecuencia { get; set; }

    [JsonProperty("fechaUltimaVisita")]
    [VectorStoreData]
    public DateTime? FechaUltimaVisita { get; set; }

    [JsonProperty("fechaUltimaVisitaCliente")]
    [VectorStoreData]
    public DateTime? FechaUltimaVisitaCliente { get; set; }

    [JsonProperty("nuevoMes")]
    [VectorStoreData]
    public bool? NuevoMes { get; set; }

    [JsonProperty("idCentroPrincipal")]
    [VectorStoreData]
    public int? IdCentroPrincipal { get; set; }

    [JsonProperty("diasSinVenir")]
    [VectorStoreData]
    public int? DiasSinVenir { get; set; }

    [JsonProperty("nSesionesAnual")]
    [VectorStoreData]
    public int? NSesionesAnual { get; set; }

    [JsonProperty("idEstado")]
    [VectorStoreData]
    public int? IdEstado { get; set; }

    [JsonProperty("idEstadoSecundario")]
    [VectorStoreData]
    public int? IdEstadoSecundario { get; set; }

    [JsonProperty("diasHastaCumple")]
    [VectorStoreData]
    public int? DiasHastaCumple { get; set; }

    [JsonProperty("diasHastaAniversario")]
    [VectorStoreData]
    public int? DiasHastaAniversario { get; set; }

    [JsonProperty("diaFrecuente")]
    [VectorStoreData]
    public int? DiaFrecuente { get; set; }

    [JsonProperty("idRangoHorario")]
    [VectorStoreData]
    public int? IdRangoHorario { get; set; }

    [JsonProperty("ranking")]
    [VectorStoreData]
    public long? Ranking { get; set; }

    [JsonProperty("syncIndicadores")]
    [VectorStoreData]
    public bool SyncIndicadores { get; set; }

    [JsonProperty("idfrequency")]
    [VectorStoreData]
    public int? Idfrequency { get; set; }

    [JsonProperty("gestor")]
    [VectorStoreData]
    public string Gestor { get; set; } = default!;

    [JsonProperty("alertaFrecuency")]
    [VectorStoreData]
    public decimal? AlertaFrecuency { get; set; }

    [JsonProperty("alertaMonetary")]
    [VectorStoreData]
    public decimal? AlertaMonetary { get; set; }

    [JsonProperty("productoPrincipal")]
    [VectorStoreData]
    public int? ProductoPrincipal { get; set; }

    [JsonProperty("idJuegoFavoritoUno")]
    [VectorStoreData]
    public int? IdJuegoFavoritoUno { get; set; }

    [JsonProperty("idJuegoFavoritoDos")]
    [VectorStoreData]
    public int? IdJuegoFavoritoDos { get; set; }

    [JsonProperty("idJuegoFavoritoMesasUno")]
    [VectorStoreData]
    public int? IdJuegoFavoritoMesasUno { get; set; }

    [JsonProperty("idJuegoFavoritoMesasDos")]
    [VectorStoreData]
    public int? IdJuegoFavoritoMesasDos { get; set; }

    [JsonProperty("saldo")]
    [VectorStoreData]
    public decimal? Saldo { get; set; }

    [JsonProperty("telefono")]
    [VectorStoreData]
    public string Telefono { get; set; } = default!;

    [JsonProperty("comunicado")]
    [VectorStoreData]
    public bool? Comunicado { get; set; }

    [JsonProperty("compartido")]
    [VectorStoreData]
    public bool? Compartido { get; set; }

    [JsonProperty("apuestas")]
    [VectorStoreData]
    public bool? Apuestas { get; set; }

    [JsonProperty("exclusionTemporal")]
    [VectorStoreData]
    public bool? ExclusionTemporal { get; set; }

    [JsonProperty("competencia")]
    [VectorStoreData]
    public bool Competencia { get; set; }

    [JsonProperty("apuestaMedia")]
    [VectorStoreData]
    public decimal? ApuestaMedia { get; set; }

    [JsonProperty("apuestaMediaAnual")]
    [VectorStoreData]
    public decimal? ApuestaMediaAnual { get; set; }

    [JsonProperty("winAnual")]
    [VectorStoreData]
    public decimal? WinAnual { get; set; }

    [JsonProperty("winMensual")]
    [VectorStoreData]
    public decimal? WinMensual { get; set; }

    [JsonProperty("duracionPromedio")]
    [VectorStoreData]
    public decimal? DuracionPromedio { get; set; }

    [JsonProperty("duracionPromedioAnual")]
    [VectorStoreData]
    public decimal? DuracionPromedioAnual { get; set; }

    [JsonProperty("coininDiario")]
    [VectorStoreData]
    public decimal? CoininDiario { get; set; }

    [JsonProperty("coininMensual")]
    [VectorStoreData]
    public decimal? CoininMensual { get; set; }

    [JsonProperty("coininAnual")]
    [VectorStoreData]
    public decimal? CoininAnual { get; set; }

    [JsonProperty("coininUltVisita")]
    [VectorStoreData]
    public decimal? CoininUltVisita { get; set; }

    [JsonProperty("coininUltSemana")]
    [VectorStoreData]
    public decimal? CoininUltSemana { get; set; }

    [JsonProperty("coinInClase2")]
    [VectorStoreData]
    public decimal? CoinInClase2 { get; set; }

    [JsonProperty("coinInClase3")]
    [VectorStoreData]
    public decimal? CoinInClase3 { get; set; }

    [JsonProperty("coinInSP30")]
    [VectorStoreData]
    public decimal? CoinInSP30 { get; set; }

    [JsonProperty("coinInSP90")]
    [VectorStoreData]
    public decimal? CoinInSP90 { get; set; }

    [JsonProperty("coinInSPAnual")]
    [VectorStoreData]
    public decimal? CoinInSPAnual { get; set; }

    [JsonProperty("coinInSemanaActual")]
    [VectorStoreData]
    public decimal? CoinInSemanaActual { get; set; }

    [JsonProperty("visitasSemanaActual")]
    [VectorStoreData]
    public int? VisitasSemanaActual { get; set; }

    [JsonProperty("puntosBaseSemanaActual")]
    [VectorStoreData]
    public decimal? PuntosBaseSemanaActual { get; set; }

    [JsonProperty("puntosBaseMesActual")]
    [VectorStoreData]
    public decimal? PuntosBaseMesActual { get; set; }

    [JsonProperty("visitasMesActual")]
    [VectorStoreData]
    public int? VisitasMesActual { get; set; }

    [JsonProperty("visitasMesAnterior")]
    [VectorStoreData]
    public int? VisitasMesAnterior { get; set; }

    [JsonProperty("coinInMesActual")]
    [VectorStoreData]
    public decimal? CoinInMesActual { get; set; }

    [JsonProperty("coinInMesAnterior")]
    [VectorStoreData]
    public decimal? CoinInMesAnterior { get; set; }

    [JsonProperty("coinInUlt7")]
    [VectorStoreData]
    public decimal? CoinInUlt7 { get; set; }

    [JsonProperty("sportium")]
    [VectorStoreData]
    public bool? Sportium { get; set; }

    // MESAS
    [JsonProperty("dropDiario")]
    [VectorStoreData]
    public decimal? DropDiario { get; set; }

    [JsonProperty("dropMensual")]
    [VectorStoreData]
    public decimal? DropMensual { get; set; }

    [JsonProperty("dropAnual")]
    [VectorStoreData]
    public decimal? DropAnual { get; set; }

    [JsonProperty("dropUltVisita")]
    [VectorStoreData]
    public decimal? DropUltVisita { get; set; }

    [JsonProperty("dropUltSemana")]
    [VectorStoreData]
    public decimal? DropUltSemana { get; set; }

    [JsonProperty("winMensualMesas")]
    [VectorStoreData]
    public decimal? WinMensualMesas { get; set; }

    [JsonProperty("winAnualMesas")]
    [VectorStoreData]
    public decimal? WinAnualMesas { get; set; }

    [JsonProperty("winMesActualMesas")]
    [VectorStoreData]
    public decimal? WinMesActualMesas { get; set; }

    [JsonProperty("winMesAnteriorMesas")]
    [VectorStoreData]
    public decimal? WinMesAnteriorMesas { get; set; }

    [JsonProperty("winDiaAnteriorMesas")]
    [VectorStoreData]
    public decimal? WinDiaAnteriorMesas { get; set; }

    [JsonProperty("winSemanaAnteriorMesas")]
    [VectorStoreData]
    public decimal? WinSemanaAnteriorMesas { get; set; }

    [JsonProperty("winSemanaActualMesas")]
    [VectorStoreData]
    public decimal? WinSemanaActualMesas { get; set; }

    [JsonProperty("visitasMesActualMesas")]
    [VectorStoreData]
    public int? VisitasMesActualMesas { get; set; }

    [JsonProperty("visitasMesAnteriorMesas")]
    [VectorStoreData]
    public int? VisitasMesAnteriorMesas { get; set; }

    [JsonProperty("visitasSemanaAnteriorMesas")]
    [VectorStoreData]
    public int? VisitasSemanaAnteriorMesas { get; set; }

    [JsonProperty("visitasSemanaActualMesas")]
    [VectorStoreData]
    public int? VisitasSemanaActualMesas { get; set; }

    [JsonProperty("visitasDiaAnteriorMesas")]
    [VectorStoreData]
    public int? VisitasDiaAnteriorMesas { get; set; }

    [JsonProperty("dropMesActual")]
    [VectorStoreData]
    public decimal? DropMesActual { get; set; }

    [JsonProperty("dropMesAnterior")]
    [VectorStoreData]
    public decimal? DropMesAnterior { get; set; }

    [JsonProperty("dropUlt7")]
    [VectorStoreData]
    public decimal? DropUlt7 { get; set; }

    [JsonProperty("dropSemanaActualMesas")]
    [VectorStoreData]
    public decimal? DropSemanaActualMesas { get; set; }

    [JsonProperty("dropDiaAnteriorMesas")]
    [VectorStoreData]
    public decimal? DropDiaAnteriorMesas { get; set; }

    [JsonProperty("idMonetaryAnualMesas")]
    [VectorStoreData]
    public int? IdMonetaryAnualMesas { get; set; }

    [JsonProperty("idMonetaryAnualDetalladoMesas")]
    [VectorStoreData]
    public int? IdMonetaryAnualDetalladoMesas { get; set; }

    [JsonProperty("idMonetaryMesas")]
    [VectorStoreData]
    public int? IdMonetaryMesas { get; set; }

    [JsonProperty("idFrequencyAnualMesas")]
    [VectorStoreData]
    public int? IdFrequencyAnualMesas { get; set; }

    [JsonProperty("idFrequencyMesas")]
    [VectorStoreData]
    public int? IdFrequencyMesas { get; set; }

    [JsonProperty("idRecencyMesas")]
    [VectorStoreData]
    public string IdRecencyMesas { get; set; } = default!;

    [JsonProperty("alertaFrecuencyMesas")]
    [VectorStoreData]
    public decimal? AlertaFrecuencyMesas { get; set; }

    [JsonProperty("alertaMonetaryMesas")]
    [VectorStoreData]
    public decimal? AlertaMonetaryMesas { get; set; }

    [JsonProperty("idCentroPrincipalMesas")]
    [VectorStoreData]
    public int? IdCentroPrincipalMesas { get; set; }

    [JsonProperty("nuevoMesMesas")]
    [VectorStoreData]
    public bool? NuevoMesMesas { get; set; }

    [JsonProperty("nSalasMesas")]
    [VectorStoreData]
    public int? NsalasMesas { get; set; }

    [JsonProperty("frecuenciaMediaMesas")]
    [VectorStoreData]
    public decimal? FrecuenciaMediaMesas { get; set; }

    [JsonProperty("frecuenciaMesas")]
    [VectorStoreData]
    public int? FrecuenciaMesas { get; set; }

    [JsonProperty("diasSinVenirMesas")]
    [VectorStoreData]
    public int? DiasSinVenirMesas { get; set; }

    [JsonProperty("nSesionesAnualMesas")]
    [VectorStoreData]
    public int? NSesionesAnualMesas { get; set; }

    [JsonProperty("diasHastaAniversarioMesas")]
    [VectorStoreData]
    public int? DiasHastaAniversarioMesas { get; set; }

    [JsonProperty("recenciaMediaMesas")]
    [VectorStoreData]
    public decimal? RecenciaMediaMesas { get; set; }

    [JsonProperty("recenciaMesas")]
    [VectorStoreData]
    public int? RecenciaMesas { get; set; }

    [JsonProperty("fechaUltimaVisitaMesas")]
    [VectorStoreData]
    public DateTime? FechaUltimaVisitaMesas { get; set; }

    [JsonProperty("saldoOnline")]
    [VectorStoreData]
    public decimal? SaldoOnline { get; set; }

    [JsonProperty("mail")]
    [VectorStoreData]
    public string Mail { get; set; } = default!;

    [JsonProperty("clubNombreCAS")]
    [VectorStoreData]
    public string ClubNombreCAS { get; set; } = default!;

    [JsonProperty("clubIdCas")]
    [VectorStoreData]
    public string ClubIdCas { get; set; } = default!;

    [JsonProperty("nivelNombreCas")]
    [VectorStoreData]
    public string NivelNombreCas { get; set; } = default!;

    [JsonProperty("nivelIdCas")]
    [VectorStoreData]
    public string NivelIdCas { get; set; } = default!;

    [JsonProperty("clubNombreMes")]
    [VectorStoreData]
    public string ClubNombreMes { get; set; } = default!;

    [JsonProperty("clubIdMes")]
    [VectorStoreData]
    public string ClubIdMes { get; set; } = default!;

    [JsonProperty("nivelNombreMes")]
    [VectorStoreData]
    public string NivelNombreMes { get; set; } = default!;

    [JsonProperty("nivelIdMes")]
    [VectorStoreData]
    public string NivelIdMes { get; set; } = default!;

    //Campo que usas como texto semántico
    [JsonProperty("descripcion")]
    [VectorStoreData]
    [TextSearchResultValue]
    public string Descripcion { get; set; } = default!;

    //Embedding 
    [Newtonsoft.Json.JsonIgnore]
    [VectorStoreVector(1536)]
    public Embedding<float> DefinitionEmbedding { get; set; } = default!;
}
