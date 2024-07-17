using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projectServer.Models.Account;

namespace projectServer.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(UserModel userModel);
    }
}