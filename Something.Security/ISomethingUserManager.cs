using System.Security.Claims;

namespace Something.Security
{
    public interface ISomethingUserManager
    {
        ClaimsPrincipal GetUserPrinciple();
        string GetUserToken();
    }
}