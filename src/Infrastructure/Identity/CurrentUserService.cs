using Infrastructure.Common.Interfaces;

namespace Infrastructure.Identity;

public class CurrentUserService : ICurrentUserService
{
    // TODO: Remove (FOR JWT)
    // private readonly IHttpContextAccessor _httpContextAccessor;
    //
    // public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    // {
    //     _httpContextAccessor = httpContextAccessor;
    // }
    //
    // public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    // TODO: For session identification probably add value for UserId when user is authorized so that user id (or User Object) can be stored in scoped service
    // SCOPED SERVICE: In this service, with every HTTP request, we get a new instance.
    // The same instance is provided for the entire scope of that request.
    // This is a better option when you want to maintain a state within a request.
    public string? UserId
    {
        get => "no user :(";
        set { throw new NotImplementedException(); }
    }
}