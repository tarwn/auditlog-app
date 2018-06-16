using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AuditLogApp.Common.DTO;

public class ViewCustomizationModel
{
    public ViewCustomizationModel() { }

    public ViewCustomizationModel(ViewCustomizationDTO customization)
    {
        Url = customization.URL;
        Logo = customization.Logo;
        Title = customization.Title;
        Copyright = customization.Copyright;
        HeaderLinks = customization.HeaderLinks.Select(hl => new ViewCustomizationHeaderLink(hl.Label, hl.URL))
                                               .ToList();
    }

    [MaxLength(400)]
    public string Url { get; set; }

    [MaxLength(400)]
    public string Logo { get; set; }

    [MaxLength(40)]
    public string Title { get; set; }

    [MaxLength(80)]
    public string Copyright { get; set; }

    [Required]
    public List<ViewCustomizationHeaderLink> HeaderLinks { get;set;}
}