using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CoffeeShop.TagHelpers
{
    [HtmlTargetElement("custom-email")]  // Target the <custom-email> tag
    public class EmailTagHelper : TagHelper
    {
        // Property to hold the email address, maps to the "address" attribute in the tag
        public string Email { get; set; }

        // Optional property for display name, if not set, it will use the email address
        public string DisplayName { get; set; }

        // Optional property for custom css class
        public string CssClass { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Set the output tag to "a" to generate an anchor tag
            output.TagName = "a";

            // Set the "href" attribute to a "mailto:" link with the email address
            output.Attributes.SetAttribute("href", $"mailto:{Email}");

            // If CssClass is provided, add it to the 'class' attribute
            if (!string.IsNullOrEmpty(CssClass))
            {
                output.Attributes.SetAttribute("class", CssClass);
            }

            // Set the link text inside the anchor tag
            output.Content.SetContent(DisplayName ?? Email);

        }
    }
}
