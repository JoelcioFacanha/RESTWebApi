using Microsoft.AspNetCore.Http;
using System.Linq;

namespace DevIO.Api.Extensions
{
    public class CustomAuthorization
    {
        public static bool ValidarClaimsUsuario(HttpContext context, string nameClaim, string claimValue)
        {
            return context.User.Identity.IsAuthenticated && context.User.Claims.Any(c => c.Type == nameClaim && c.Value.Contains(claimValue));
        }
    }
}
