using System.Text.RegularExpressions;
using ActiveReportsRtfToHtmlConverter.Models;

namespace ActiveReportsRtfToHtmlConverter.Helpers;

public static class TemplateFinder
{
    public static List<FileResult> ReplaceInTemplateFiles(FileInfo file, FileResult fileResults)
    {
        var results = new List<FileResult>();

        var templates = FileHelper.GetRelatedTemplateFiles(file);
        int i = 0;
        foreach (var template in templates)
        {
            string content = File.ReadAllText(template.FullName);
            foreach (var rtfField in fileResults.RtfFields)
            {
                var newFieldName = rtfField[..^4] + ".HTML";
                string updatedContent = Regex.Replace(content, Regex.Escape(rtfField), newFieldName);
                content = updatedContent;
            }
            File.WriteAllText($"{template.DirectoryName}/test{i++}.cs", content);
        }
        


        return results;
    }
}