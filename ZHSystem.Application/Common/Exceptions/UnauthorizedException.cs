using Microsoft.AspNetCore.Http;

namespace ZHSystem.Application.Common.Exceptions
{
    public class UnauthorizedException : AppException
      {
        public UnauthorizedException(string message) : base(message) { }
        public override int StatusCode => StatusCodes.Status401Unauthorized;
    }
}
