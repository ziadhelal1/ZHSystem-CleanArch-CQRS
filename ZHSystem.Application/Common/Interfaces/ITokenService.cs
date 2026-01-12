using ZHSystem.Application.DTOs.Auth;
using ZHSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Application.Common.Interfaces
{
    public interface ITokenService
    {
        
        string GenerateAccessToken(User user, IList<string> roles);

        
        RefreshToken CreateRefreshToken(int userId);

        
        int AccessTokenExpirationMinutes { get; }

    }
}
