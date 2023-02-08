using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspDotNetSummerProject.Models
{
    public class TempClass
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please insert your Password")]
        [MinLength(8, ErrorMessage = "Minimum lenght 8")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password not matching")]
        [Required(ErrorMessage = "Re-insert your password")]
        [MinLength(8, ErrorMessage = "Minimum lenght 8")]
        public string ConfirmPassword { get; set; }


    }
}