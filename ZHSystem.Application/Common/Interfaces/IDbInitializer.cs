using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Application.Common.Interfaces
{
    public interface IDbInitializer
    {
        Task SeedAsync();
    }

}
