using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibrarySystem.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlContent Truncate(this IHtmlHelper htmlHelper, string input, int length = 50)
        {
            if (string.IsNullOrWhiteSpace(input))
                return HtmlString.Empty;

            var truncated = input.Length > length
                ? input.Substring(0, length) + "..."
                : input;

            return new HtmlString(truncated);
        }
    }
}
