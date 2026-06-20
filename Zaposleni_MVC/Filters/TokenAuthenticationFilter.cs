using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Zaposleni_Clean_MVC_API.Filters
{
    public class TokenAuthenticationFilter : ActionFilterAttribute // Primena ovim atributom na kontroler zamenjuje proveru putem metode CheckToken() za pojedinačne akcije
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var token = httpContext.Session.GetString("JwtToken");

            // Dohvatanje trenutnog kontrolera i akcije
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            // Preskakanje provere za Login i Register akcije u AuthController
            if (controller == "Auth" && (action == "Login" || action == "Register"))
            {
                return; // Ne proverava token za ove akcije
            }

            // Preusmeravanje na Login ako token nije prisutan
            if (string.IsNullOrEmpty(token))
            {
                var returnUrl = httpContext.Request.Path; // Čuvanje trenutnog URL-a
                context.Result = new RedirectToActionResult("Login", "Auth", new { returnUrl }); // Preusmeravanje na Login akciju sa trenutnim URL-om
            }
        }
    }

}
