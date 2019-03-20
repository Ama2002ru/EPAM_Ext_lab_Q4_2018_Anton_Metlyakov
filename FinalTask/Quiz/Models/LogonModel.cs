namespace Quiz.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Модель данных для логона пользователя
    /// </summary>
    public class LogonModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string UserName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}