using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Application.Common.Exceptions
{
    public class ValidationException:AppException
    {
        public IDictionary<string, string[]> Errors { get; }
        public ValidationException(IDictionary<string, string[]> errors) : base("One or more validation failures have occurred.") 
        {
            Errors=errors;
        }
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}
