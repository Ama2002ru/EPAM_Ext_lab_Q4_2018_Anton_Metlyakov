// <copyright file="bundleconfig.cs" company="Epam Ext Lab">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace My_Calc
{
    using System.Web;
    using System.Web.Optimization;
  
    /// <summary>
    ///  Class definition
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        /// </summary>
        /// <param name="bundles"> Still not getting so far...</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
