// <copyright file="filterconfig.cs" company="Epam Ext Lab">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace My_Calc
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Class definition
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// method definition
        /// </summary>
        /// <param name="filters">some filters to apply</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
