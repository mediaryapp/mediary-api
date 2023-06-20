using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Common.Extensions;

public static class SignInResultExtension
{
    public static Result ToApplicationResult(this SignInResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(new List<string>(){"failed"});
    }
}
