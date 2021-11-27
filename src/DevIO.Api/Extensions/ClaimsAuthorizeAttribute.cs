using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DevIO.Api.Extensions
{
    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string nameClaim, string claimValue)
            : base(typeof(RequisitoClaimFilter))
        {
            Arguments = new object[] { new Claim(type: nameClaim, claimValue) };
        }
    }
}
