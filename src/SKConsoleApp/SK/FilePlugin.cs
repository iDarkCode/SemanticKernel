using Microsoft.SemanticKernel;
using SemanticKernel.Services;
using System.ComponentModel;

namespace SemanticKernel.SK;

public class FilePlugin(IFileService fileService)
{
    private readonly IFileService _fileService = fileService;

    [KernelFunction("create_file")]
    [Description("Creates a file requesting a name for the file")]
    public void CreateFile(string fileName)
    {
        _fileService.CreateFile(fileName);
    }

    [KernelFunction("delete_file")]
    public void DeleteFile(string fileName)
    {
        _fileService.DeleteFile(fileName);
    }

    [KernelFunction("read_file")]
    public string ReadFile(string filePath)
        => _fileService.ReadFile(filePath);

    [KernelFunction("write_file")]
    public void WriteFile(string fileName, string content)
    {
        _fileService.WriteFile(fileName, content);
    }
}
