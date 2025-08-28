#pragma warning disable SKEXP0001

using Microsoft.SemanticKernel.Data;
using Microsoft.Extensions.VectorData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticKernel.Domain;

public class Faq
{
    [VectorStoreKey]
    [TextSearchResultName]
    public string key { get; set; }
    [VectorStoreData(IsFullTextIndexed = true)]
    public string category { get; set; }
    [VectorStoreData]
    public string question { get; set; }
    [VectorStoreData]
    [TextSearchResultValue]
    public string definition { get; set; }
    [VectorStoreVector(1536)]
    public ReadOnlyMemory<float> definitionEmbedding { get; set; } 

}
