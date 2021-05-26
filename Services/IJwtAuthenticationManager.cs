using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zembil.Services
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(int userid);
        string Decrypt(string authHeader);
    }
}
