using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Application.DTOs
{
    public record RegisterResponseDto
    {
        public string Message { get; init; } = default!;
    }
}
