using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Providers.Interfaces
{
    public interface IJwtTokenProvider
    {
        public Task<string> GenerateTokenAsync(string userId);
    }
}
