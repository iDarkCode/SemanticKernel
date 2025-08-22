#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

using SemanticKernel.Services;
using SemanticKernel.SK;
using SemanticKernel.VectorStore;
using Microsoft.SemanticKernel;
using ModelContextProtocol.Client;

var openAIKey = Environment.GetEnvironmentVariable("SKCourseOpenAIKey");
//var azureRegion = Environment.GetEnvironmentVariable("SKCourseAzureRegion");
//var azureKey = Environment.GetEnvironmentVariable("SKCourseAzureKey");

// 1. Configurar transporte SSE hacia el servidor MCP
//var transportOptions = new SseClientTransportOptions
//{
//    Endpoint = new Uri("http://localhost:5000") // URL del MCP
//};
//var transport = new SseClientTransport(transportOptions);

// 2. Crear cliente MCP
//var mcpClient = await McpClientFactory.CreateAsync(transport);

// 3. Obtener herramientas desde el servidor
//var tools = await mcpClient.ListToolsAsync();

// 4. Configurar Semantic Kernel
var kernelBuilder = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion("gpt-4o-mini-2024-07-18", $"{openAIKey}")
    //.AddOpenAITextToImage($"{openAIKey}", modelId: "dall-e-3")
    //.AddOpenAITextToAudio("tts-1", $"{openAIKey}")
    ;
kernelBuilder.Services.AddSingleton<InvoiceService>();
kernelBuilder.Services.AddSingleton<AggregationService>();
kernelBuilder.Services.AddSingleton<VectorSearchService>();
kernelBuilder.Services.AddSingleton<InvoicePlugin>();

kernelBuilder.Services.AddLogging();

// Registrar las herramientas como funciones en SK
//kernelBuilder.Plugins.AddFromFunctions("Invoices", tools.Select(t => t.AsKernelFunction()));

var kernel = kernelBuilder.Build();

var plugin = kernel.Services.GetRequiredService<InvoicePlugin>();
kernel.Plugins.AddFromObject(plugin, "invoices");


#region ConsoleChat

#endregion


// Obtener estado de una factura específica
await AskQuestionAsync("¿Cuál es el estado de la factura INV-0023?");

// Buscar facturas de un cliente en un rango de fechas
await AskQuestionAsync("Muéstrame las facturas de CUST003 entre enero y marzo 2024");

// Buscar todas las facturas de un cliente sin rango
await AskQuestionAsync("Muéstrame todas las facturas de CUST002");


async Task AskQuestionAsync(string question)
{
    Console.WriteLine($"Pregunta: {question}");

    // SK analizará la pregunta y seleccionará la función correcta
    var result = await kernel.InvokePromptAsync(question);

    Console.WriteLine($"Respuesta: {result}\n");
}

public partial class Program { }
