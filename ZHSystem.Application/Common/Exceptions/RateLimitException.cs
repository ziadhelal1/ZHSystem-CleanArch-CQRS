using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Application.Common.Exceptions
{
    public class RateLimitException : AppException
    {
        public RateLimitException(string message) : base(message) { }
        public override int StatusCode => StatusCodes.Status429TooManyRequests;//429;
    }
}
