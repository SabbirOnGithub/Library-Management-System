using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Web.Models.ViewModels
{
    public class RegisterViewModel: LoginViewModel
    {
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
