using System.Collections.Concurrent;
using System.Reflection;

namespace App.Components.Services.AssetManager;

public static class AssetManager
{
    // Cache for assembly names to avoid repeated reflection calls.
    private readonly static ConcurrentDictionary<Assembly , string> AssemblyNameCache = new ConcurrentDictionary<Assembly , string>();

    /// <summary>
    ///     Builds a resource path string by obtaining the calling assembly's name and combining it
    ///     with the provided path segments and file name.
    /// </summary>
    /// <param name="fileName">The file name to be appended (e.g. "logo.png").</param>
    /// <param name="pathSegments">A variable number of strings representing the sub-path segments.</param>
    /// <returns>
    ///     A formatted resource path in the format:
    ///     _content/AssemblyName/pathSegments/fileName
    /// </returns>
    public static string LoadAssets(string fileName , params string[] pathSegments)
    {
        pathSegments ??= [];

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name must be provided." , nameof(fileName));

        var callingAssembly = Assembly.GetCallingAssembly();

        var assemblyName = AssemblyNameCache.GetOrAdd(callingAssembly ,
            asm => asm.GetName().Name ?? throw new InvalidOperationException("Assembly name could not be determined."));

        var parentPath = Path.Combine(pathSegments);

        var finalPath = Path.Combine("_content" , assemblyName , parentPath , fileName).Replace(Path.DirectorySeparatorChar , '/');

        return finalPath;
    }
}