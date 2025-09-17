#pragma warning disable SKEXP0010
#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0050

using HandlebarsDotNet;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.InMemory;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SK.Course;
using SK.Course.Plugins;
using Spectre.Console;
using System.Text.Json;


IChatCompletionService _chatCompletionService = default!;
ChatHistory _chatHistory = default!;
OpenAIPromptExecutionSettings _executionSettings = default!;
InMemoryCollection<string, CustomerRFMDto>? _vectorCollection = null;
Kernel _kernel = default!;

// Inicialización
_kernel = await InitializeAsync();
ShowWelcomeMessage();
await ChatLoopAsync(_kernel);

// Chat loop principal
async Task ChatLoopAsync(Kernel kernel)
{
    var settings = new KernelArguments(_executionSettings);

    while (true)
    {
        var userInput = AnsiConsole.Ask<string>("[blue]User:[/]");

        if (IsExitCommand(userInput))
        {
            AnsiConsole.MarkupLine("[bold red]Desconectando del chat...[/]");
            break;
        }

        _chatHistory.AddUserMessage(userInput);

        await GenerateAssistantResponse(kernel, settings, userInput);
    }
}

bool IsExitCommand(string input) 
    => input.Equals("exit", StringComparison.OrdinalIgnoreCase);

// Generación de respuesta del asistente
async Task GenerateAssistantResponse(Kernel kernel, KernelArguments settings, string userQuery)
{
    try
    {
        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Balloon)
            .StartAsync("Pensando...", async _ =>
            {
                //Generar embedding de la consulta
                var embedder = kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
                var queryEmbedding = await embedder.GenerateAsync(userQuery);

                //Recuperar contexto desde vector store
                string context = await BuildContextAsync(queryEmbedding);

                //Preparar prompt usando template
                string prompt = await BuildPrompt(userQuery, context);

                var reducer = new ChatHistoryTruncationReducer(targetCount: 4, thresholdCount: 8);
                var reducedHistory = await _chatHistory.ReduceAsync(reducer, CancellationToken.None);

                //Invocar LLM con function calling: el LLM decide si usar plugin o vector store
                var response = await _chatCompletionService.GetChatMessageContentAsync(
                    prompt,
                    _executionSettings,
                    kernel
                );

                var text = response?.Items
                    .OfType<Microsoft.SemanticKernel.TextContent>()
                    .Select(t => t.Text)
                    .FirstOrDefault() ?? "[Sin Respuesta]";

                //Agregar respuesta Llm
                _chatHistory.AddAssistantMessage(text);

                //Mostrar salida
               // AnsiConsole.MarkupLine($"[bold yellow]Prompt generado:[/]\n{prompt}\n");
            });
       
        var lastMessage = _chatHistory.LastOrDefault();
        AnsiConsole.MarkupLine($"[bold green]Assistant:[/] {lastMessage}\n");
    }
    catch (Exception ex)
    {
        AnsiConsole.MarkupLine($"[bold red]Error:[/] {ex.Message}\n");
    }
}

// Construcción del contexto semántico
async Task<string> BuildContextAsync(Embedding<float> queryEmbedding)
{
    if (_vectorCollection == null) 
        return "";

    var contextBuilder = new System.Text.StringBuilder();
    var searchResults = _vectorCollection.SearchAsync(
        queryEmbedding, 
        top: 3, 
        options: new VectorSearchOptions<CustomerRFMDto>());
    
    int count = 0;
    await foreach (var item in searchResults)
    {
        count++;
        if (count > 3) 
            break;
        
        contextBuilder.AppendLine($"- Cliente {item.Record.NumCliente}, " +
                                  $"  Saldo={item.Record.Saldo}, " +
                                  $"  Frecuencia={item.Record.Frecuencia}");
       
    }

    return contextBuilder.ToString();
}

// Template de prompt
Task<string> BuildPrompt(string userQuery, string context)
{
    var template = @"
Eres un asistente experto en clientes de casinos. 
Tienes acceso a información de clientes y contexto relevante. 
Usa la información de contexto solo si es necesaria.

Contexto relevante:
{{context}}

Pregunta del usuario:
{{userQuery}}

Responde de forma clara, indicando el cliente, saldo, frecuencia y cualquier dato relevante.
Si necesitas más información, puedes invocar las funciones disponibles en el plugin.
";

    var compiledTemplate = Handlebars.Compile(template);

    var rendered = compiledTemplate(new
    {
        userQuery,
        context
    });

    return Task.FromResult(rendered);
}

// Inicialización Kernel
async Task<Kernel> InitializeAsync()
{
    var openAIKey =  Environment.GetEnvironmentVariable("SKCourseOpenAIKey");
    
    if (string.IsNullOrEmpty(openAIKey))
        throw new InvalidOperationException("Environment variable 'SKCourseOpenAIKey' is missing.");

    //Configuración
    var builder = Kernel.CreateBuilder()
        .AddOpenAIChatCompletion("gpt-4o-mini-2024-07-18", openAIKey)
        .AddOpenAIEmbeddingGenerator("text-embedding-ada-002", openAIKey);

    //Registrar servicios DI
    builder.Services.AddInMemoryVectorStore();
    builder.Services.AddSingleton<CustomerRfmPlugin>();
    builder.Services.AddSingleton<IPromptTemplateFactory, KernelPromptTemplateFactory>();
       
    //Registrar plugin
    builder.Plugins.AddFromType<CustomerRfmPlugin>();

    var kernel = builder.Build();

    //Inicializar vector store
    var collection = new InMemoryCollection<string, CustomerRFMDto>("customers");
    await collection.EnsureCollectionExistsAsync();

    //Cargar datos
    var rfms = JsonSerializer.Deserialize<List<CustomerRFMDto>>(File.ReadAllText("../../../Resources/customers.json"))!.ToList();

    var random = new Random();
    var shuffledRfms = rfms.OrderBy(_ => random.Next()).Take(200).ToList();

    var embedder = kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

    const int concurrency = 4;
    using var semaphore = new SemaphoreSlim(concurrency);

    var embedTasks = shuffledRfms.Select(async c =>
    {
        await semaphore.WaitAsync();
        try
        {
            var text = BuildCustomerText(c);
            c.DefinitionEmbedding = await embedder.GenerateAsync(text);
        }
        finally
        {
            semaphore.Release();
        }
    });

    await Task.WhenAll(embedTasks);

    await collection.UpsertAsync(shuffledRfms);

    _executionSettings = new OpenAIPromptExecutionSettings
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(autoInvoke: false),
        MaxTokens = 1000,
        Temperature = 0.2
    };

    _vectorCollection = collection;
    _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
    _chatHistory = new ChatHistory("Eres un asistente útil");

    return kernel;
}

string BuildCustomerText(CustomerRFMDto c) =>
    $"Cliente {c.NumCliente}, Sesión {c.Sesion}, Saldo {c.Saldo}, Frecuencia {c.Frecuencia}, Última visita {c.FechaUltimaVisita}.";

void ShowWelcomeMessage()
{
    AnsiConsole.MarkupLine("[bold green]Bienvenido Semantic Kernel Chat POC!![/]");
    AnsiConsole.MarkupLine("Opciones:");
    AnsiConsole.MarkupLine(" - Escribe texto y pulsa Enter");
    AnsiConsole.MarkupLine(" - Escribe [red]exit[/] para salir.\n");
}

public partial class Program { }
