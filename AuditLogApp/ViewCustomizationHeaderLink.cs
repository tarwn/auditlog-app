using System.ComponentModel.DataAnnotations;

public class ViewCustomizationHeaderLink
{
    public ViewCustomizationHeaderLink() { }

    public ViewCustomizationHeaderLink(string label, string url)
    {
        Label = label;
        Url = url;
    }

    [Required]
    [MaxLength(40)]
    public string Label { get; set; }

    [Required]
    [MaxLength(400)]
    public string Url { get; set; }
}