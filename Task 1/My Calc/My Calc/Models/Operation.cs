using My_Calc.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace My_Calc.Models
{
    public enum Operation
    {
        [Display(Name = "Add", ResourceType = typeof(CalcResources))]
        Add
    }
}