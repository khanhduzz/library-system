using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LibrarySystem.TagHelpers
{
    [HtmlTargetElement("admin-only")]
    public class AdminOnlyTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminOnlyTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated || !user.IsInRole("Admin"))
            {
                // Suppress rendering if not Admin
                output.SuppressOutput();
                return;
            }

            var childContent = await output.GetChildContentAsync();
            output.Content.SetHtmlContent(childContent);
        }
    }
}

