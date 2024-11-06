using System.Data;
using System.Xml;
using ActiveReportsRtfToHtmlConverter.Models;
using System.Text;
using RtfPipe;

namespace ActiveReportsRtfToHtmlConverter.Helpers;

public static class ResourceReader
{
    public static FileResult ProcessFile(FileInfo file)
    {
        var result = new FileResult() { FileName = file.Name };
        
        try
        {
            // Load the resx file
            var document = new XmlDocument();
            document.Load(file.FullName);

            var nodes = document.SelectNodes("//data[@name]");
            if (nodes != null)
            {
                foreach (XmlNode oldNode in nodes)
                {
                    try
                    {
                        var nodeName = oldNode.Attributes?["name"]?.Value;
                        if (nodeName == null) continue;

                        if (nodeName.EndsWith(".rtf", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var nodeNameWithoutExtension = nodeName[..^4];
                            result.RtfFields.Add(nodeName);
                            var rtfContent = oldNode.InnerText;
                            var shortenedContent = rtfContent.Length > 64 ? rtfContent[..63] : rtfContent;
                            Console.WriteLine($"Replacing: {nodeNameWithoutExtension} - {shortenedContent}");

                            var newNode = document.CreateElement(nodeNameWithoutExtension + ".HTML");
                            var html = Rtf.ToHtml(rtfContent);
                            newNode.InnerText = html;
                            var parent = oldNode.ParentNode;
                            parent?.ReplaceChild(newNode, oldNode);
                        }
                    }
                    catch (Exception e)
                    {
                        result.Error = e.Message;
                        
                        // error occurred, break out of processing this file.
                        break;
                    }
                }

                if (file.DirectoryName != null) document.Save(Path.Combine(file.DirectoryName, "test.resx"));
            }
            
            // // If there were changes save the file
            // if (output.Duplicates > 0)
            // {
            //     rootDoc.Save(fileInfo.FullName);
            //     SummarizeFile(output);
            // }
            // else
            //     NoFileChanges(output);
        }
        catch (Exception e)
        {
            result.Error = e.Message;
            Console.WriteLine(e.ToString());
        }

        return result;
    } 
}