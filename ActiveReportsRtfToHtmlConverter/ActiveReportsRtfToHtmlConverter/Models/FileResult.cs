namespace ActiveReportsRtfToHtmlConverter.Models;

public class FileResult
{
    private List<string> _rtfFields = new();
    public required string FileName { get; set; }

    public List<string> RtfFields
    {
        get => _rtfFields;
        set => _rtfFields = value;
    }

    public IDictionary<string, int> DesignerReferences { get; set; } = new Dictionary<string, int>();
    public IDictionary<string, int> BaseReferences { get; set; } = new Dictionary<string, int>();

    public string? Error { get; set; } // right now just catch the first error and abort 
}