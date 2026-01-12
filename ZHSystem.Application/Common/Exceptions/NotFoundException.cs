using Microsoft.AspNetCore.Http;

namespace ZHSystem.Application.Common.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message) : base(message) { }
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}
