using System.Text;
using ActiveReportsRtfToHtmlConverter.Helpers;

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

foreach (var resx in resxFiles)
{
    LogHelper.LogWithTimestamp($"Started processing {resx.Name}.");
    var fileResult = ResourceReader.ProcessFile(resx);
}




