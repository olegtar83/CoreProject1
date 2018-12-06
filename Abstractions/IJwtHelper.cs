using System.Collections.Generic;
using System.Security.Claims;

namespace webapp
{
    public interface IJwtHelper
    {
        string getToken(List<Claim> claims);
    }
}