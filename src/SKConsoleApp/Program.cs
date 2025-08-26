#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Connectors.Google;
using Microsoft.SemanticKernel.Connectors.Ollama;
using SemanticKernel.Services;
using SemanticKernel.SK;
using SemanticKernel.VectorStore;
using Spectre.Console;
using System.Runtime.CompilerServices;

IChatCompletionService _chatCompletionService;
ChatHistory _chatHistory;
OpenAIPromptExecutionSettings _executionSettings;


Kernel kernel = InitializeKernels();
ShowWelcomeMessage();

await RunChatLoopAsync(kernel);

async Task RunChatLoopAsync(Kernel kernel)
{
    KernelArguments _settings = new(_executionSettings);

    while (true)
    {
        var userInput = AnsiConsole.Ask<string>("[blue]User:[/]");
        if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            AnsiConsole.MarkupLine("[bold red]Desconectando del chat...[/]");
            break;
        }
        else if (userInput.Equals("img", StringComparison.OrdinalIgnoreCase))
        {
            var imagePathOrUrl = AnsiConsole.Prompt(
                new TextPrompt<string>("[blue]Introduzca la ruta de la imagen (local) o URL:[/]")
                    .Validate(pathOrUrl =>
                    {
                        if (string.IsNullOrWhiteSpace(pathOrUrl))
                            return ValidationResult.Error("[red]La ruta o URL de la imagen no puede estar vacía.[/]");
                        return ValidationResult.Success();
                    })
            );

            var additionalText =
                AnsiConsole.Ask<string>("[blue](Deja vacío si no quieres añadir texto):[/]");

            var userMessageContents = CreateUserContentAsync(additionalText, imagePathOrUrl);
            if (userMessageContents is null)
                continue;

            _chatHistory.AddUserMessage(userMessageContents);
        }
        else
        {
            _chatHistory.AddUserMessage(userInput);
        }

        try
        {
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Balloon)
                .StartAsync("Pensando...", async _ =>
                {
                    ChatMessageContent? response = await _chatCompletionService
                        .GetChatMessageContentAsync(_chatHistory, _executionSettings, kernel);

                    var text = response?.Items
                        .OfType<TextContent>()
                        .Select(t => t.Text)
                        .FirstOrDefault() ?? "[Sin Respuesta]";

                    // var result = await kernel.InvokePromptAsync(userInput, _settings);
                    //var text = result.ToString();

                    _chatHistory.AddAssistantMessage(text);
                });

            var lastMessage = _chatHistory.LastOrDefault();
            AnsiConsole.MarkupLine($"[bold green]Assistant:[/] {lastMessage}\n");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {ex.Message}\n");
        }
    }
}

ChatMessageContentItemCollection? CreateUserContentAsync(string additionalText, string imagePathOrUrl)
{
    var contents = new ChatMessageContentItemCollection
    {
        !string.IsNullOrWhiteSpace(additionalText)
        ? new TextContent(additionalText)
        : new TextContent("Dame la descripción de la imagen")
    };

    if (imagePathOrUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
        imagePathOrUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
    {
        contents.Add(new ImageContent(new Uri(imagePathOrUrl)));
    }
    else
    {
        AnsiConsole.MarkupLine($"[grey]Leyendo imagen desde ruta local...[/]");
        if (!File.Exists(imagePathOrUrl))
        {
            AnsiConsole.MarkupLine($"[red]La ruta de la imagen '{imagePathOrUrl}' no existe[/]");
            return null;
        }
        try
        {
            var imageBytes = File.ReadAllBytes(imagePathOrUrl);
            var mimeType = InferMimeType(imagePathOrUrl);
            contents.Add(new ImageContent(imageBytes, mimeType));
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error al leer la imagen: {ex.Message}[/]");
            return null;
        }
    }

    return contents;
}

Kernel InitializeKernels()
{
    var openAIKey = Environment.GetEnvironmentVariable("SKCourseOpenAIKey");
    if (string.IsNullOrEmpty(openAIKey))
        throw new InvalidOperationException("Environment variable 'SKCourseOpenAIKey' is missing.");

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
        //.AddGoogleAIGeminiChatCompletion(modelId: "gemini-2.5-flash", apiKey: openAIKey)
        .AddOpenAIChatCompletion("gpt-4o-mini-2024-07-18", $"{openAIKey}")
        //.AddOpenAITextToImage($"{openAIKey}", modelId: "dall-e-3")
        //.AddOpenAITextToAudio("tts-1", $"{openAIKey}")
        ;

    //Registrar las herramientas como funciones en SK
    //kernelBuilder.Plugins.AddFromFunctions("Invoices", tools.Select(t => t.AsKernelFunction()));

    kernelBuilder.Services.AddSingleton<InvoiceService>();
    kernelBuilder.Services.AddSingleton<AggregationService>();
    kernelBuilder.Services.AddSingleton<VectorSearchService>();
    kernelBuilder.Services.AddSingleton<InvoicePlugin>();
    kernelBuilder.Services.AddLogging();

    kernelBuilder.Plugins.AddFromType<SystemInfoPlugin>();
    kernelBuilder.Plugins.AddFromObject(new FilePlugin(new FileService()));

    var kernel = kernelBuilder.Build();
    
    KernelPlugin systemInfoPlugin = KernelPluginFactory.CreateFromType<SystemInfoPlugin>();

    // registrar el plugin una vez construido
    var plugin = kernel.Services.GetRequiredService<InvoicePlugin>();
    kernel.Plugins.AddFromObject(plugin, "Facturas");


    foreach (var p in kernel.Plugins)
    {
        Console.WriteLine($"Plugin: {p.Name}");
        foreach (var f in p)
        {
            Console.WriteLine($" - {f.Name}");
        }
    }

    _executionSettings = new OpenAIPromptExecutionSettings
    {
        //    MaxTokens = 2048,
        //    Temperature = 0.5,
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
    };

    _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
    _chatHistory = new ChatHistory("Eres un asistente útil");

    return kernel;
}

void ShowWelcomeMessage()
{
    AnsiConsole.MarkupLine("[bold green]Bienvenido Semantic Kernel Chat!![/]");
    AnsiConsole.MarkupLine("Opciones:");
    AnsiConsole.MarkupLine(" - Escribe texto y pulsa Enter");
    //AnsiConsole.MarkupLine(" - Escribe [blue]img[/] para adjuntar una imagen (ruta local o URL)");
    //AnsiConsole.MarkupLine(" - Escribe [red]exit[/] para salir.\n");
}

string InferMimeType(string filePath)
{
    var extension = Path.GetExtension(filePath).ToLowerInvariant();
    return extension switch
    {
        ".png" => "image/png",
        ".jpg" or ".jpeg" => "image/jpeg",
        ".gif" => "image/gif",
        _ => "image/jpeg"
    };
}


//// Obtener estado de una factura específica
//await AskQuestionAsync("¿Cuál es el estado de la factura INV-0023?");

//// Buscar facturas de un cliente en un rango de fechas
//await AskQuestionAsync("Muéstrame las facturas de CUST003 entre enero y marzo 2024");

//// Buscar todas las facturas de un cliente sin rango
//await AskQuestionAsync("Muéstrame todas las facturas de CUST002");


//async Task AskQuestionAsync(string question)
//{
//    Console.WriteLine($"Pregunta: {question}");

//    // SK analizará la pregunta y seleccionará la función correcta
//    var result = await kernel.InvokePromptAsync(question);

//    Console.WriteLine($"Respuesta: {result}\n");
//}

public partial class Program { }
