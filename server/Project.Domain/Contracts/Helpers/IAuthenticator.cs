using Project.Domain.Aggregates.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Contracts.Helpers
{
     public interface IAuthenticator
    {
        User Authenticate(string username, string password);
        void ValidatePassword(string password);
        string GenerateUserToken(User user, string purpose);
        bool VerifyUserToken(User user, string purpose, string token);
        string GenerateJsonWebToken(User user, string key, string issuer);
        IEnumerable<Claim> ExtractClaimsFromToken(string token);
    }
}
