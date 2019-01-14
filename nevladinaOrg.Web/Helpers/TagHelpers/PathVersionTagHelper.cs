using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Razor.TagHelpers;

namespace nevladinaOrg.Web.Helpers.TagHelpers
{
    [HtmlTargetElement(Attributes = AppendVersionAttributeName)]
    public class PathVersionTagHelper : TagHelper
    {
        private const string AppendVersionAttributeName = "path-version";
        private readonly string[] _supportedSourceAttributes = { "src", "link" };

        [HtmlAttributeName(AppendVersionAttributeName)]
        public bool AppendVersion { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!AppendVersion)
                return base.ProcessAsync(context, output);

            foreach (var item in _supportedSourceAttributes)
                if (output.Attributes.TryGetAttribute(item, out var attribute))
                    output.Attributes.SetAttribute(item, $"{attribute.Value}?v={Guid.NewGuid().ToString().ToLower()}");

            return base.ProcessAsync(context, output);
        }
    }
}