using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Identity;
using System.Collections.Generic;
using System.Linq;

public class ViewConfigurationModel{
    public ViewConfigurationModel() { }

    public ViewConfigurationModel(ViewDTO view)
    {
        Id = view.Id;
        AccessKey = view.AccessKey;
        Custom = new ViewCustomizationModel(view.Customization);
        Columns = view.Columns.Select(c => new ViewColumnConfigurationModel(c))
                              .ToList();
    }

    public  ViewId Id { get;set;}
    public string AccessKey { get; set; }
    public ViewCustomizationModel Custom { get; set; }
    public List<ViewColumnConfigurationModel> Columns { get; set; }
}