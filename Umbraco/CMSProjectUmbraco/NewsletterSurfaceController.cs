using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Cms.Infrastructure.Scoping;
using NPoco;

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
            var notSignedUp = true;

            using (var scope = _scopeProvider.CreateScope())
            {
                var query = new Sql().Select("*").From("Newsletter").Where("Email == (@n)", new { n = formData.Email });

                var result = scope.Database.Fetch<CMSProjectUmbraco.AddCommentsTable.Newsletterchema>(query);

                if (result.Count > 0)
                {
                    TempData["Newsletter"] = "You are already signed up for the newsletter";
                    notSignedUp = false;
                }
                scope.Complete();
            }

            if (notSignedUp)
            {
                using (var scope = _scopeProvider.CreateScope())
                {
                    var data = new CMSProjectUmbraco.AddCommentsTable.Newsletterchema();
                    data.Email = formData.Email;
                    scope.Database.Insert(data);
                    scope.Complete();
                    TempData["Newsletter"] = "You have signed up for the newsletter!";
                }
            }

            return RedirectToCurrentUmbracoPage();
        }
    }
}