namespace Quiz
{
    using System.Web;
    using System.Web.Mvc;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        // https://www.dotnetcurry.com/aspnet-mvc/1068/aspnet-mvc-exception-handling
        // https://docs.microsoft.com/ru-ru/aspnet/web-api/overview/error-handling/exception-handling
    }
}
