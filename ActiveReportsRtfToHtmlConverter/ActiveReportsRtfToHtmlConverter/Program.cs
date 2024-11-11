using System.Text;
using ActiveReportsRtfToHtmlConverter.Helpers;
using ActiveReportsRtfToHtmlConverter.Models;

System.Text.EncodingProvider ppp = System.Text.CodePagesEncodingProvider.Instance;
Encoding.RegisterProvider(ppp);


var startTime = DateTime.Now;

if (Environment.GetCommandLineArgs().Length == 0)
    throw new ArgumentNullException($"Please pass an absolute path for the utility to check.");
    
var path = Environment.GetCommandLineArgs()[1];

LogHelper.LogWithTimestamp($"Started looking in {path} for resx files.");

// start with the resx files in the directory and work backwards over to the designer or cs files
var resxFiles = FileHelper.GetResourceFiles(path);
LogHelper.LogWithTimestamp($"Found {resxFiles.Count} resx files.");

var totals = new List<FileResult>();
foreach (var resx in resxFiles)
{
    LogHelper.LogWithTimestamp($"Started processing {resx.Name}.");
    totals.Add(ResourceReader.ProcessFile(resx));
}
LogHelper.LogWithTimestamp($"File processing finished.");


// summarize things
var rtfFieldCount = 0;
var resxChanged = 0;
var designerFilesChanged = 0;
var designerFileReferences = 0;
var baseFilesChanged = 0;
var baseFileReferences = 0;
foreach (var template in totals)
{
    // may want to further report things at some point.
    if (template.RtfFields.Count > 0)
    {
        resxChanged++;
        rtfFieldCount += template.RtfFields.Count;
    }
    if (template.BaseReferences.Count > 0)
    {
        baseFilesChanged++;
        baseFileReferences += template.BaseReferences.Count;
    }
    if (template.DesignerReferences.Count > 0)
    {
        designerFilesChanged++;
        designerFileReferences += template.DesignerReferences.Count;
    }
}

var timespan = DateTime.Now - startTime;
LogHelper.LogWithTimestamp($"Run completed, total processing time was: {timespan}.");
LogHelper.LogWithTimestamp($"{rtfFieldCount} RTF fields found across {resxChanged} resource files out of {resxFiles.Count}.");
LogHelper.LogWithTimestamp($"{designerFileReferences} RTF references found across {designerFilesChanged} designer files.");
LogHelper.LogWithTimestamp($"{baseFileReferences} RTF references found across {baseFilesChanged} designer files.");


