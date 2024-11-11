namespace ActiveReportsRtfToHtmlConverter.Helpers;

public static class FileHelper
{
    // this is not recursive intentionally right now.
    public static List<FileInfo> GetResourceFiles(string path)
    {
        var rootDirectory = new DirectoryInfo(path);
        
        var resourceFiles = new List<FileInfo>();

        resourceFiles.AddRange(rootDirectory.GetFiles("*.resx"));

        return resourceFiles;
    }
    
    // this is not recursive intentionally right now.
    public static List<FileInfo> GetRelatedTemplateFiles(FileInfo file)
    {
        var templateFiles = new List<FileInfo>();
        if (file.DirectoryName == null) return templateFiles;
        
        var rootDirectory = new DirectoryInfo(file.DirectoryName);
        var extensionlessFile = Path.GetFileNameWithoutExtension(file.Name);

        templateFiles.AddRange(rootDirectory.GetFiles($"*{extensionlessFile}.cs"));
        templateFiles.AddRange(rootDirectory.GetFiles($"*{extensionlessFile}.designer.cs"));
        return templateFiles;
    }
}