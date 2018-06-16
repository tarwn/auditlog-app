public class ViewCustomizationHeaderLink
{
    public ViewCustomizationHeaderLink() { }

    public ViewCustomizationHeaderLink(string label, string url)
    {
        Label = label;
        Url = url;
    }

    public string Label { get; set; }
    public string Url { get; set; }
}