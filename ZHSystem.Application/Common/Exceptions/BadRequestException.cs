using Microsoft.AspNetCore.Http;

namespace ZHSystem.Application.Common.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException(string message) : base(message) { }
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}
