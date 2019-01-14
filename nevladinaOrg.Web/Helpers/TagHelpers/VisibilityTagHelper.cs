using System.Threading.Tasks;

using Microsoft.AspNetCore.Razor.TagHelpers;

namespace nevladinaOrg.Web.Helpers.TagHelpers
{
    [HtmlTargetElement(Attributes = VisibleAttributeName)]
    public class VisibilityTagHelper : TagHelper
    {
        private const string VisibleAttributeName = "is-visible";

        [HtmlAttributeName(VisibleAttributeName)]
        public bool Visible { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if(!Visible)
                output.SuppressOutput();
            
            return base.ProcessAsync(context, output);
        }
    }
}