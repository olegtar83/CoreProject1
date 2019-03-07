using System.Collections.Generic;
using System.Security.Claims;

namespace webapp.Abstractions
{
    public interface IJwtHelper
    {
        string getToken(List<Claim> claims);
    }
}