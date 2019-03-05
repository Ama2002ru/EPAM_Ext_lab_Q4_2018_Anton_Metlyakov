namespace Quiz.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class SearchModel
    {
        [StringLength(100, MinimumLength = 2)]
        [Required]
        public string Name { get; set; }
    }
}