using My_Calc.Helpers;
using My_Calc.Models;
using My_Calc.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace My_Calc.Controllers
{
    public class CalcController : Controller
    {
        const double DefaultX = 0;
        const double DefaultY = 0;
        const string DefaultResult = "";

        public static List<string> Results = new List<string>();

        // GET: Calc
        public ActionResult Index()
        {
            return View();
        }
/*
        [Obsolete]
        public ActionResult OldAdd(int x, int y)
        {
            return View(x + y);
        }
*/
        /// <summary>
        /// Initialize View ?
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View(new CalcModel() { X = DefaultX , Y = DefaultY, Result = DefaultResult });
        }

        /// <summary>
        /// Simple arithmetics on two double variables
        /// </summary>
        /// <param name="model"></param>
        /// <returns> Not yet learned which type of object it returns</returns>
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
                              throw new DivideByZeroException();
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
            return View(model);
        }

    }
}