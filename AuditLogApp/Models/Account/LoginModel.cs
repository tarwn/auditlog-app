using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Models.Account
{
    public class LoginModel
    {
        [Required]
        [StringLength(72)]
        public string UserName { get; set; }

        [Required]
        [StringLength(72)]
        public string Password { get; set; }
    }
}
