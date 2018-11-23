// <copyright file="calcmodel.cs" company="Epam Ext Lab">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace My_Calc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using My_Calc.Resources;

    /// <summary>
    /// Class Calcmodel
    /// </summary>
    public class CalcModel
    {
        [Display(Name = "x", ResourceType = typeof(CalcResources))]
        [Required]   
        [Range(-double.MaxValue, double.MaxValue)]

        /// <summary>
        /// Gets or sets First argument
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets second argument
        /// </summary>
        [Display(Name = "y", ResourceType = typeof(CalcResources))]
        [Required]
        [Range(-double.MaxValue, double.MaxValue)]
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets Result string
        /// </summary>
        [Display(Name = "Result", ResourceType = typeof(CalcResources))]
        public string Result { get; set; }

        /// <summary>
        /// Gets or sets operation selector
        /// </summary>
        [Display(Name = "OpName", ResourceType = typeof(CalcResources))]
        public Operation Op { get; set; }
     }
}