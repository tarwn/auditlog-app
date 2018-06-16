using AuditLogApp.Common.DTO;
using System.Collections.Generic;
using System.Linq;

public class ViewColumnConfigurationModel
{
    public ViewColumnConfigurationModel() { }

    public ViewColumnConfigurationModel(ViewColumnDTO column)
    {
        Order = column.Order;
        Label = column.Label;
        Lines = column.Lines.Select(line => new ViewColumnConfigurationLineModel(line.Field))
                            .ToList();
    }

    public int Order { get; set; }
    public string Label { get; set; }
    public List<ViewColumnConfigurationLineModel> Lines { get; set; }
}