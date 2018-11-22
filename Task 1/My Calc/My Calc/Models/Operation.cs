/// <summary>
/// test StyleCop
/// </summary>

namespace My_Calc.Models
{
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using My_Calc.Resources;

    public enum Operation
    {
        /// <summary>
        /// 
        /// </summary>
        
        [Display(Name = "Add", ResourceType = typeof(CalcResources))]
        Add,
        [Display(Name = "Substract", ResourceType = typeof(CalcResources))]
        Substract,
        [Display(Name = "Multiply", ResourceType = typeof(CalcResources))]
        Multiply,
        [Display(Name = "Divide", ResourceType = typeof(CalcResources))]
        Divide
    }
}