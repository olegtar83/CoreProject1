using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace webapp
{
    public static class BasicExtensions
    {

        public static string GetClaimValueByType(this List<Claim> claims, string claimType)
        {
            try
            {
                return claims.FirstOrDefault(x => x.Type == claimType)?.Value;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
    }
}
