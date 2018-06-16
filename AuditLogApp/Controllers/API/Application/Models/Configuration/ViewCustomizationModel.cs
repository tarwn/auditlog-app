using System.Collections.Generic;
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

    public string Url { get; set; }
    public string Logo { get; set; }
    public string Title { get; set; }
    public string Copyright { get; set; }
    public List<ViewCustomizationHeaderLink> HeaderLinks { get;set;}
}