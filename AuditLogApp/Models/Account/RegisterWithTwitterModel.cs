using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuditLogApp.Models.Account
{
    public class RegisterWithTwitterModel
    {
        [Required]
        [StringLength(72)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(80)]
        public string CompanyName { get; set; }

        // Below are set from twitter SSO and should not be modified by user

        [Required]
        public string TwitterId { get; set; }

        [Required]
        public string TwitterUsername { get; set; }
    }
}
