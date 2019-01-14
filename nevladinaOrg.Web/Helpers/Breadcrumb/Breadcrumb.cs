using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;

using nevladinaOrg.Web.Constants;

namespace nevladinaOrg.Web.Helpers.Breadcrumb
{
    /* TO-DO: UKLONITI */
    public class Breadcrumb : IBreadcrumb
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Breadcrumb(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddItem(string url, string label)
        {
            var items = GetExistingItems() ?? new List<BreadcrumbItem>();
            items.Add(new BreadcrumbItem
            {
                Url = url?.ToLower(),
                Label = label
            });

            _httpContextAccessor.HttpContext.Session.SetObject(SessionKeys.Breadcrumb, items);
        }

        public void AddItem(string controller, string action, string label)
        {
            controller = controller.Replace("Controller", "");

            var items = GetExistingItems() ?? new List<BreadcrumbItem>();
            items.Add(new BreadcrumbItem
            {
                Url = $"/{controller}/{action}".ToLower(),
                Label = label
            });

            _httpContextAccessor.HttpContext.Session.SetObject(SessionKeys.Breadcrumb, items);
        }

        public HtmlString Display(bool clear = true)
        {
            var existingItems = GetExistingItems();
            if(existingItems == null)
                return HtmlString.Empty;

            var html = "<ul class=\"page-breadcrumb\">";

            for (var i = 0; i < existingItems.Count; i++)
            {
                var item = existingItems.ElementAt(i);

                if (i != existingItems.Count - 1)
                    html += string.IsNullOrWhiteSpace(item.Url) ? $"<li><span>{item.Label}</span><i class=\"fa fa-circle\"></i></li>" : $"<li><a href=\"{item.Url}\">{item.Label}</a><i class=\"fa fa-circle\"></i></li>";
                else
                    html += string.IsNullOrWhiteSpace(item.Url) ? $"<li><span>{item.Label}</span></li>" : $"<li><a href=\"{item.Url}\">{item.Label}</a></li>";
            }

            html += "</ul>";

            if(clear)
                ClearExistingItems();

            return new HtmlString(html);
        }

        public void ClearExistingItems()
        {
            _httpContextAccessor.HttpContext.Session.RemoveObject(SessionKeys.Breadcrumb);
        }

        private ICollection<BreadcrumbItem> GetExistingItems()
        {
            return _httpContextAccessor.HttpContext.Session.GetObject<ICollection<BreadcrumbItem>>(SessionKeys.Breadcrumb);
        }

        private class BreadcrumbItem
        {
            public string Url { get; set; }
            public string Label { get; set; }
        }
    }
}