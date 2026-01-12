using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Application.DTOs.Auth
{
    public class RefreshResponseDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;  
        public DateTime ExpiresAt { get; set; }
    }
}
