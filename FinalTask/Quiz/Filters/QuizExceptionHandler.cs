namespace Quiz
{
    using System;
    using System.Data.Common;
    using System.Web.Mvc;
    using DAL;

    public class QuizExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled &&
            filterContext.Exception is Exception)
            {
                Logger.Error(string.Format("{0} {1}\n", filterContext.Exception.Message, filterContext.Exception.Source));
                filterContext.Result =
                new ViewResult
                {
                    ViewName = "~/Views/Shared/ErrorPage.cshtml",
                    ViewData = new ViewDataDictionary<ExceptionContext>(filterContext)
                };
                filterContext.ExceptionHandled = true;
            }
        }
    }
}