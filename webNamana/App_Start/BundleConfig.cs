using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace webNamana.App_Start
{
        public static class BundleConfig
        {
            public static void RegisterBundles(BundleCollection bundles)
            {
                // Основной стиль
                bundles.Add(new StyleBundle("~/bundles/main/css").Include(
                          "~/Content/style.css", new CssRewriteUrlTransform()));

                // Bootstrap стили
                bundles.Add(new StyleBundle("~/bundles/bootstrap/css").Include(
                          "~/Content/bootstrap.min.css", new CssRewriteUrlTransform()));

                // Font Awesome
                bundles.Add(new StyleBundle("~/bundles/font-awesome/css").Include(
                          "~/Content/font-awesome.min.css", new CssRewriteUrlTransform()));

                // Toastr (уведомления)
                bundles.Add(new StyleBundle("~/bundles/toaster/css").Include(
                          "~/Vendors/toastr/toastr.min.css", new CssRewriteUrlTransform()));

                // DataTables (таблицы)
                bundles.Add(new StyleBundle("~/bundles/datatables/css").Include(
                          "~/Vendors/datatables/datatables.min.css", new CssRewriteUrlTransform()));

                // jQuery
                bundles.Add(new ScriptBundle("~/bundles/jquery/js").Include(
                          "~/Scripts/jquery-3.4.1.min.js"));

                // Bootstrap JS
                bundles.Add(new ScriptBundle("~/bundles/bootstrap/js").Include(
                          "~/Scripts/bootstrap.min.js"));

                // Toastr JS
                bundles.Add(new ScriptBundle("~/bundles/toaster/js").Include(
                          "~/Vendors/toastr/toastr.min.js"));

                // DataTables JS
                bundles.Add(new ScriptBundle("~/bundles/datatables/js").Include(
                          "~/Vendors/datatables/datatables.min.js"));

                // jQuery Validation
                bundles.Add(new ScriptBundle("~/bundles/validation/js").Include(
                          "~/Scripts/jquery.validate.min.js"));

                // jQuery Unobtrusive AJAX
                bundles.Add(new ScriptBundle("~/bundles/unobtrusive/js").Include(
                          "~/Scripts/jquery.unobtrusive-ajax.min.js"));
            }
        }
    }
