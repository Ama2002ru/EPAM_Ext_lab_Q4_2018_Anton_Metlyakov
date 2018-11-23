// <copyright file="calccontroller.cs" company="Epam Ext Lab">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace My_Calc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;    
    using My_Calc.Helpers;
    using My_Calc.Models;
    using My_Calc.Resources;

    /// <summary>
    /// Class Cacl controller
    /// </summary>
    public class CalcController : Controller
    {
        /// <summary>
        /// Const Default X
        /// </summary>
        public const double DefaultX = 0;

        /// <summary>
        /// Const default Y
        /// </summary>
        public const double DefaultY = 0;

        /// <summary>
        /// Const Default Res
        /// </summary>
        public const string DefaultResult = "";

        /// <summary>
        ///  This is implicit static constructor ?
        /// </summary>
        public static List<string> Results = new List<string>();

        /// <summary>
        /// // GET: Calc
        /// </summary>
        /// <returns>view result</returns>
        public ActionResult Index()
        {
            return this.View();
        }
 
         /// <summary>
         ///  'Add' action
         /// </summary>
         /// <returns>View result</returns>
            public ActionResult Add()
        {
            return this.View(new CalcModel() { X = DefaultX, Y = DefaultY, Result = DefaultResult });
        }

        /// <summary>
        /// Simple arithmetics on two double variables
        /// </summary>
        /// <param name="model"> Calc model</param>
        /// <returns>View result</returns>
        [HttpPost]
        public ActionResult Add(CalcModel model)
        {
            double result = 0;
            try
            {
                switch (model.Op)
                {
                    case Operation.Add:
                        result = model.X + model.Y;
                        break;
                    case Operation.Substract:
                        result = model.X - model.Y;
                        break;
                    case Operation.Multiply:
                        result = model.X * model.Y;
                        break;
                    case Operation.Divide:
                        result = model.X / model.Y;
                        if (double.IsInfinity(result) || double.IsNaN(result))
                        {
                            throw new DivideByZeroException();
                        }

                        break;
                }

                model.Result = string.Format("{0}     {1} {3} {2} = {4}\n",
                                      DateTime.Now.ToString("dd MMMM HH:MM", CultureInfo.InvariantCulture),
                                      model.X.ToString(),
                                      model.Y.ToString(),
                                      model.Op.DisplayName(),
                                      result);
            }
            catch (OverflowException)
            {
                 model.Result = string.Format("{0}     {1}\n",
                                      DateTime.Now.ToString("dd MMMM HH:MM", CultureInfo.InvariantCulture),
                                      CalcResources.OverFlow);
            }
            catch (DivideByZeroException)
            {
                model.Result = string.Format("{0}     {1}\n",
                                      DateTime.Now.ToString("dd MMMM HH:MM", CultureInfo.InvariantCulture),
                                      CalcResources.DivideByZero);
            }

            Results.Add(model.Result);
            return this.View(model);
        }
    }
}