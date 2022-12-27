using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AppAspNetCore.Extentions
{
    public static class IdentityExtentions
    {
        public static string GetAccountId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity) identity).FindFirst("AccountId");
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetRoleId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity) identity).FindFirst("RoleId");
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetUserName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity) identity).FindFirst("UserName");
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetEmail(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity) identity).FindFirst("Email");
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetAvatar(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity) identity).FindFirst("Avatar");
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetSpecificClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == claimType);
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}
