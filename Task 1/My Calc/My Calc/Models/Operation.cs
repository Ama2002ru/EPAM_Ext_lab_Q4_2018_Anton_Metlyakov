// <copyright file="Operation.cs" company="Epam Ext Lab">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace My_Calc.Models
{
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using My_Calc.Resources;

    /// <summary>
    /// List of operation supported by Calculator solution
    /// </summary>
    public enum Operation
    {
        /// <summary>
        /// Add sub-item
        /// </summary>
        [Display(Name = "Add", ResourceType = typeof(CalcResources))]
        Add,

        /// <summary>
        /// substract sub-item
        /// </summary>
        [Display(Name = "Substract", ResourceType = typeof(CalcResources))]
        Substract,

        /// <summary>
        /// Multiply sub-item
        /// </summary>       
        [Display(Name = "Multiply", ResourceType = typeof(CalcResources))]
        Multiply,

        /// <summary>
        /// Divide sub-item
        /// </summary>
        [Display(Name = "Divide", ResourceType = typeof(CalcResources))]
        Divide
    }
}