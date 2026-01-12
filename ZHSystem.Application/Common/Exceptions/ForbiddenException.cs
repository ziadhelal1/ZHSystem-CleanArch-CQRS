using Microsoft.AspNetCore.Http;

namespace ZHSystem.Application.Common.Exceptions
{
    public class ForbiddenException : AppException
    {
        
        public ForbiddenException()
            : base("Access Denied: You do not have permission to access this resource.") { }

        public ForbiddenException(string message) : base(message) { }

        public override int StatusCode => StatusCodes.Status403Forbidden;
    }
}
