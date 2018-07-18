using AuditLogApp.Common.DTO;
using AuditLogApp.Common.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AuditLogApp.Controllers.API.Public.Models.Configuration
{
    public class ViewConfigurationModel
    {
        public ViewConfigurationModel() { }

        public ViewConfigurationModel(ViewDTO view)
        {
            Id = view.Id.RawValue;
            AccessKey = view.AccessKey;
            Custom = new ViewCustomizationModel(view.Customization);
            Columns = view.Columns.Select(c => new ViewColumnConfigurationModel(c))
                                  .ToList();
        }

        [Required]
        public Guid Id { get; set; }

        public string AccessKey { get; set; }

        [Required]
        public ViewCustomizationModel Custom { get; set; }

        [Required]
        public List<ViewColumnConfigurationModel> Columns { get; set; }
    }
}