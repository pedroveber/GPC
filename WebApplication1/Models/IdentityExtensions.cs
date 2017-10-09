using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Principal;

namespace App.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetIdGuilda(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("IdGuilda");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}