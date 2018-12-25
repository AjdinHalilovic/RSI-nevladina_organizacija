using Microsoft.AspNetCore.Html;

namespace nevladinaOrg.Web.Helpers.AjaxFlashMessage
{
    public interface IAjaxFlashMessage
    {
        void Success(string message);
        void Danger(string message);
        IHtmlContent Display();
    }
}