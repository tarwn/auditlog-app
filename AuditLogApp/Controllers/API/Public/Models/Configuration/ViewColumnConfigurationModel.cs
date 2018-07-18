using AuditLogApp.Common.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AuditLogApp.Controllers.API.Public.Models.Configuration
{
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

        [Required]
        public int Order { get; set; }

        [Required]
        [MaxLength(40)]
        public string Label { get; set; }

        [Required]
        public List<ViewColumnConfigurationLineModel> Lines { get; set; }
    }
}