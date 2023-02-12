using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project.Domain.Aggregates.User;
using Project.Domain.Contracts.Helpers;
using Project.Domain.Settings;
using Project.Infrastructure.Data.Models;
using Project.Infrastructure.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Utils.Helpers
{
    public class Authenticator : IAuthenticator
    {
        private readonly UserManager<UseDataModel> _userManager;
        private readonly AppSettings _appSettings;

        public Authenticator(UserManager<UseDataModel> userManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        public User Authenticate(string username,string password)
        {
            try
            {
                var userEntity = _userManager.FindByNameAsync(username).Result;
                var match= _userManager.CheckPasswordAsync(userEntity, password).Result;
                if (!match) throw new FailAuthenticationAttemptException("Username and password do not match");
                return userEntity.ToEntity();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new FailAuthenticationAttemptException("Username and password do not match");
            }
        }
        public void ValidatePassword(string password)
        {
            var message = new List<string>();
            var validators = _userManager.PasswordValidators;
            foreach(var validator in validators)
            {
                var result = validator.ValidateAsync(_userManager, null, password).Result;
                if (result.Succeeded) continue;
                message.AddRange(result.Errors.Select(error => error.Description));
            }
            if (message.Any())
                throw new FailAuthenticationAttemptException($"Password fails the following validations: {message.Aggregate("", (message, m) => message += $"/n{m}")}");
        }

        public string GenerateJsonWebToken(User user, string key, string issuer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString(CultureInfo.InvariantCulture)),
                },
                expires : DateTime.Now.AddDays(3),
                signingCredentials: credentials);
                
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateUserToken(User user, string purpose)
        {
            var userDataModel = _userManager.FindByIdAsync(user.Id.ToString()).Result;
            if(user ==null) throw new ArgumentNullException($"User {user.UserName} not found");
            var token = _userManager.GenerateUserTokenAsync(
               user: userDataModel,
               tokenProvider: "Default",
               purpose: purpose
           ).Result;
            return token;
        }
        public bool VerifyUserToken(User user, string purpose, string token)
        {
            var userDataModel = _userManager.FindByIdAsync(user.Id.ToString()).Result;
            if (user == null) throw new ArgumentNullException($"User {user.UserName} not found");
            return _userManager.VerifyUserTokenAsync(
                user: userDataModel,
                tokenProvider: "Default",
                purpose: purpose,
                token: token
            ).Result;
        }
        public IEnumerable<Claim> ExtractClaimsFromToken(string token) => ValidateToken(token).Claims;
        /// <summary>
        /// Validate and return decoded token, throw exceptions if token is invalid
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private JwtSecurityToken ValidateToken(string token)
        {
            new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _appSettings.Jwt.Issuer,
                ValidAudience = _appSettings.Jwt.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Jwt.Key))
            }, out var validatedToken);
            return validatedToken as JwtSecurityToken;
        }
    }
}
