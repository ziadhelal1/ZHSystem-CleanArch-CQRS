using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Application.Common.Exceptions
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = null!;
        public string? Details { get; set; }
        public IDictionary<string, string[]> Errors { get; set; } = null!;
    }
}
