using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Cms.Infrastructure.Scoping;

namespace CMSProjectUmbraco
{
    public class NewsletterDTO
    {
        public string Email { get; set; }
    }

    public class NewsletterSurfaceController : SurfaceController
    {
        private IScopeProvider _scopeProvider;
        public NewsletterSurfaceController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider,
            IScopeProvider scopeProvider)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            this._scopeProvider = scopeProvider;
        }

        public IActionResult newsletterSubmit(NewsletterDTO formData)
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                var data = new CMSProjectUmbraco.AddCommentsTable.Newsletterchema();
                data.Email = formData.Email;
                scope.Database.Insert(data);
                scope.Complete();
                ViewBag.Newsletter = "You have signed up for the newsletter!";
            }

            ViewBag.Newsletter = "You are already signed up for the newsletter";
            return RedirectToCurrentUmbracoPage();
        }
    }
}