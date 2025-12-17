using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace StudentCourseRegistration.Controllers
{
    public class LanguageController : Controller
    {
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            // Set the culture cookie
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    Path = "/",
                    SameSite = SameSiteMode.Lax
                }
            );

            // ✅ التأكد من أن returnUrl صحيح
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }

            return Redirect(returnUrl);
        }
    }
}