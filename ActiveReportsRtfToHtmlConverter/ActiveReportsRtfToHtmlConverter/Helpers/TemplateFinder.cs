using System.Text.RegularExpressions;
using ActiveReportsRtfToHtmlConverter.Models;

namespace ActiveReportsRtfToHtmlConverter.Helpers;

public static class TemplateFinder
{
    public static void ReplaceInTemplateFiles(FileInfo file, FileResult fileResults)
    {
        var templates = FileHelper.GetRelatedTemplateFiles(file);
        foreach (var template in templates)
        {
            var content = File.ReadAllText(template.FullName);
            
            LogHelper.LogWithTimestamp($"Looking in {template.FullName} for references.");
            foreach (var rtfField in fileResults.RtfFields)
            {
                var count = 0;
                var newFieldName = GetNewFieldName(rtfField);
                var updatedContent = Regex.Replace(content, Regex.Escape(rtfField), match =>
                {
                    count++;
                    return newFieldName;
                });
                if (count > 0)
                {
                    LogHelper.LogWithTimestamp($"Found {count} references of {rtfField}.");
                    content = updatedContent;
                        
                    if (template.FullName.EndsWith(".designer.cs", StringComparison.InvariantCultureIgnoreCase))
                        fileResults.DesignerReferences.Add(rtfField, count);
                        
                    if (template.FullName.EndsWith(".cs", StringComparison.InvariantCultureIgnoreCase))
                        fileResults.BaseReferences.Add(rtfField, count);
                }
                else
                {
                    LogHelper.LogWithTimestamp($"Nothing found for {rtfField}.");
                }
            }
            
            File.WriteAllText(template.FullName, content);
        }
    }

    private static string GetNewFieldName(string name)
    {
        return name[..^4] + ".Html";
    }
}