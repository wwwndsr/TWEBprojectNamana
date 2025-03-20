using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;

namespace webNamana.App_Start
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Основные стили
            bundles.Add(new StyleBundle("~/bundles/main/css")
                .Include("~/Content/style.css",
                         "~/Content/responsive.css"));

            // Bootstrap стили
            bundles.Add(new StyleBundle("~/bundles/bootstrap/css")
                .Include("~/Content/bootstrap.css"));

            // jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery/js")
                .Include("~/Scripts/jquery-3.4.1.min.js"));

            // Bootstrap JS
            bundles.Add(new ScriptBundle("~/bundles/bootstrap/js")
                .Include("~/Scripts/bootstrap.js"));

            // Включение оптимизации (можно отключить для отладки)
            BundleTable.EnableOptimizations = false;
        }
    }
}
