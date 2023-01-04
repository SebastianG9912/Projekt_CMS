using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CMSProjectUmbraco;
using Umbraco.Cms.Infrastructure.Scoping;
using NPoco;

namespace CMSProjectUmbraco.Newsletters
{
    public class NewsletterViewComponent : ViewComponent
    {

        private IScopeProvider _scopeProvider;

        public NewsletterViewComponent(IScopeProvider scopeProvider)
        {
            this._scopeProvider = scopeProvider;
        }
        public IViewComponentResult Invoke()
        {
            List<NewsletterDTO>? newsletters;

            using (var scope = _scopeProvider.CreateScope())
            {
                var query = new Sql().Select("*").From("Newsletters");

                var result = scope.Database.Fetch<CMSProjectUmbraco.AddCommentsTable.Newsletterchema>(query);

                newsletters = new List<NewsletterDTO>();

                foreach (var data in result)
                {
                    var model = new NewsletterDTO()
                    {
                        Email = data.Email
                    };

                    newsletters.Add(model);
                }
                scope.Complete();
            }

            return View(newsletters);
        }
    }
}
