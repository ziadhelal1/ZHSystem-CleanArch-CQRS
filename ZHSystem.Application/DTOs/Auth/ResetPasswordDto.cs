using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZHSystem.Application.DTOs.Auth
{
    public class ResetPasswordDto
    {
        public string Token { get; set; } = null!;
        
        public string NewPassword { get; set; } = null!;
    }

}
