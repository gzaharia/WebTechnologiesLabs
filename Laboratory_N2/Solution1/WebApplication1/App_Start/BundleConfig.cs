using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace WebApplication1.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
            bundles.Add(new StyleBundle("~/bundles/footer/css")
                .Include("~/Content/footer_distributed.css", new CssRewriteUrlTransform()));
            bundles.Add(new StyleBundle("~/bundles/logo/css")
                  .Include("~/Content/logo.css", new CssRewriteUrlTransform()));
            bundles.Add(new StyleBundle("~/bundles/style/css")
                  .Include("~/Content/style.css", new CssRewriteUrlTransform()));
            bundles.Add(new StyleBundle("~/bundles/file/css")
                  .Include("~/Content/file.css", new CssRewriteUrlTransform()));
        }
    }
   
}