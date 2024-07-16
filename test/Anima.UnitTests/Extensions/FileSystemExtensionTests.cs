using System.IO.Abstractions;
using Anima.Extensions;

namespace Anima.UnitTests.Extensions;

public class FileSystemExtensionTests
{
    [Fact]
    public void CombineNormalized_WithAbnormalPath_ReturnsNormalizedPath()
    {
        var fileSystem = new FileSystem();
        var directory = @"C:\Test/Folder";

        var result = fileSystem.Path.CombineNormalized(directory, "subfolder", "File.txt");

        var s = fileSystem.Path.DirectorySeparatorChar;
        Assert.Equal($"C:{s}Test{s}Folder{s}subfolder{s}File.txt", result);
    }
}