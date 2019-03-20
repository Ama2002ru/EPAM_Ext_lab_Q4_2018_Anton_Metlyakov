namespace Quiz.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Класс для ввода строки поиска чего-либо
    /// </summary>
    public class SearchModel
    {
        [StringLength(100, MinimumLength = 1)]
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}