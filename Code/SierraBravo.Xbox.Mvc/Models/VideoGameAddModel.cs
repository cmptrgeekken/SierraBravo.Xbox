using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SierraBravo.Xbox.Common.DTOs;

namespace SierraBravo.Xbox.Mvc.Models
{
    public class VideoGameAddModel
    {
        [Required]
        [StringLength(255,MinimumLength = 1)]
        [Display(Name = "Game Title")]
        public string Title { get; set; }
    }
}