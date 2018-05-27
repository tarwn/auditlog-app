using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Controllers.API.Application.Models
{
    public class CreateAPIKeyModel
    {
        [Required]
        [StringLength(80)]
        public string DisplayName { get; set; }
    }
}
