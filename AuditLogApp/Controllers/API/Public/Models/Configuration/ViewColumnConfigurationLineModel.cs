using System.ComponentModel.DataAnnotations;

namespace AuditLogApp.Controllers.API.Public.Models.Configuration
{
    public class ViewColumnConfigurationLineModel
    {
        public ViewColumnConfigurationLineModel() { }

        public ViewColumnConfigurationLineModel(string field)
        {
            Field = field;
        }

        public string Field { get; set; }
    }
}