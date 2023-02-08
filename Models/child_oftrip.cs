using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AspDotNetSummerProject.Models.Db;

namespace AspDotNetSummerProject.Models
{
    public class child_oftrip
    {
        [Required]
        public string start_location { get; set; }
        [Required]
        public string end_location { get; set; }
    }
}