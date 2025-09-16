#pragma warning disable SKEXP0001

using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Data;
using Newtonsoft.Json;

namespace SemanticKernel.Domain;

public class Faq
{
    [JsonProperty("key")]
    [VectorStoreKey]
    [TextSearchResultName]
    public string Key { get; set; } = default!;
    [JsonProperty("category")]
    [VectorStoreData]
    public string Category { get; set; } = default!;
    [JsonProperty("question")]
    [VectorStoreData]
    public string Question { get; set; } = default!;
    [JsonProperty("answer")]
    [VectorStoreData]
    [TextSearchResultValue]
    public string Answer { get; set; } = default!;
    [JsonIgnore]
    [VectorStoreVector(1536)]
    public Embedding<float> DefinitionEmbedding { get; set; } = default!;

}
