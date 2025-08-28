using Microsoft.SemanticKernel;
using System.Diagnostics;
using System.Text.Json;


namespace SemanticKernel;

internal class FunctionInvocationFilter : IFunctionInvocationFilter
{
    public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
    {
        if(context.Function.PluginName == "SearchPlugin")
        {
            Debug.WriteLine($"{context.Function.Name}:{JsonSerializer.Serialize(context.Arguments)}\n");
        }
        await next(context);
    }
}
