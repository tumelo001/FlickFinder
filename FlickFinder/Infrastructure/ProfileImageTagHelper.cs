using FlickFinder.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FlickFinder.Infrastructure
{
    [HtmlTargetElement("img", Attributes = "profile-user")]
    public class ProfileImageTagHelper : TagHelper
    {
        private UserManager<AppUser> _userManager;


        public ProfileImageTagHelper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HtmlAttributeName("profile-user")]
        public string UserName { get; set; }

        public override async Task ProcessAsync(TagHelperContext context,
            TagHelperOutput output)
        {
            AppUser user = await _userManager.FindByNameAsync(UserName);

            if (user != null)
            {
                output.Attributes.SetAttribute("src", $"{user.AvatarImageURL}");
                output.Attributes.SetAttribute("class", "img-thumbnail rounded-circle");
                output.Attributes.SetAttribute("style", "height:32px;width:32px");
            }

        }
    }
}
