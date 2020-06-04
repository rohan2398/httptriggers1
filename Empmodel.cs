using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
    
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;



namespace Localdatabase.Models
{
    public class Empmodel

    {
        [Display(Name = "Employee Id")]
        public int Empid { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Branch is required.")]
        public string Branch { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        public string Mobilenumber { get; set; }

        [Required(ErrorMessage = "Zip is required.")]
        public string Zip { get; set; }

    }

}