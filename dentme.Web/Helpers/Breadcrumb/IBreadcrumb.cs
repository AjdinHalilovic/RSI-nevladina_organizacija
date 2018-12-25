using Microsoft.AspNetCore.Html;

namespace nevladinaOrg.Web.Helpers.Breadcrumb
{
    public interface IBreadcrumb
    {
        void AddItem(string url, string label);
        void AddItem(string controller, string action, string label);
        void ClearExistingItems();
        HtmlString Display(bool clear = true);
    }
}