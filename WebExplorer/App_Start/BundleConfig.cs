using System.Web;
using System.Web.Optimization;

namespace WebExplorer
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //скрипты проекта
            bundles.Add(new ScriptBundle("~/bundles/application").Include(
                            "~/Scripts/common.js"
                            ));

            //редактор
            bundles.Add(new ScriptBundle("~/bundles/fileExplorer").Include(
                "~/Scripts/fileExplorer.model.js",
                "~/Scripts/fileExplorer.viewmodel.js"
                            ));
            
            //чат
            bundles.Add(new ScriptBundle("~/bundles/chat").Include(
                "~/Scripts/chat.model.js",
                "~/Scripts/chat.viewmodel.js"
                            ));

            //Main CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                "~/Content/site-responsive.css"));

            //Libs
            bundles.Add(
                new StyleBundle("~/Content/libs").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/bootstrap-responsive.css",
                    "~/Content/wbbtheme.css",
                    "~/Content/lightbox.css"
                    ));
            bundles.Add(new StyleBundle("~/Content/themes-base").Include(
                    "~/Content/themes/base/jquery.ui.core.css",
                    "~/Content/themes/base/jquery.ui.resizable.css",
                    "~/Content/themes/base/jquery.ui.selectable.css",
                    "~/Content/themes/base/jquery.ui.accordion.css",
                    "~/Content/themes/base/jquery.ui.autocomplete.css",
                    "~/Content/themes/base/jquery.ui.button.css",
                    "~/Content/themes/base/jquery.ui.dialog.css",
                    "~/Content/themes/base/jquery.ui.slider.css",
                    "~/Content/themes/base/jquery.ui.tabs.css",
                    "~/Content/themes/base/jquery.ui.datepicker.css",
                    "~/Content/themes/base/jquery.ui.progressbar.css",
                    "~/Content/themes/base/jquery.ui.theme.css"
                ));

            //Libs
            bundles.Add(
                new ScriptBundle("~/bundles/libs").Include(
                    "~/Scripts/modernizr-*",
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/jquery.cookie.js",
                    "~/Scripts/jquery-ui-{version}.js",
                    "~/Scripts/jquery.unobtrusive*",
                    "~/Scripts/jquery.validate*",
                    "~/Scripts/jquery.wysibb*",
                    "~/Scripts/jquery.placeholder.js",
                    "~/Scripts/knockout-{version}.js",
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/bootstrap.validate.js",
                    "~/Scripts/lightbox.js"
                    ));

            //отключим изза оперы
            foreach (var bundle in bundles)
            {
                bundle.Transforms.Clear();
            }
        }
    }
}