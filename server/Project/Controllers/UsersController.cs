using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Project.Domain.Aggregates.User;
using Project.Domain.Contracts.Responsitoties;
using Project.Domain.Settings;
using Project.Infrastructure.Utils.Helpers;
using Project.Web.Requests.CreateRequests.User;
using Project.Web.Responses;
using Project.Web.Responses.CreateResponses;
using Project.Web.ViewModels;
using System;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userReposity;
        private readonly Authenticator _authenticator;
        private readonly AppSettings _appSettings;
        public UsersController (
            IMapper mapper,
            IUserRepository userRepository,
            Authenticator authenticator,
            IOptions<AppSettings> appSettings
            )
        { 
            _mapper = mapper;
            _userReposity = userRepository;
            _authenticator = authenticator;
            _appSettings = appSettings.Value;
        }

        public static UsersController CreateInstance(IMapper mapper, IUserRepository userRepository, Authenticator authenticator, IOptions<AppSettings> appSettings) 
        { 
            return new UsersController (mapper, userRepository, authenticator, appSettings); 
        }

        /// <summary>
        /// Tạo tài khoản người dùng
        /// </summary>
        /// <returns></returns>
        [HttpPost("register")]
        public CreateResponses<UserViewModel> Register([FromBody] RegisterRequest request)
        {
            var user = _userReposity.GetUserByUserName(request.Username);
            if (user != null) throw new ArgumentException($"User name {request.Username} already taken");
            _authenticator.ValidatePassword(request.Password);
            user = new User(Guid.NewGuid())
            {
                UserName = request.Username,
                FullName = request.Fullname,
                Dob = request.Dob,
                EditDate = request.EditDate,
                CreateDate = request.CreateDate,
                IsActive = request.IsActive,
                PhoneNumber = request.PhoneNumber,
            };
            try
            {
                _userReposity.AddUser(user, request.Password);
            }catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return new CreateResponses<UserViewModel>
            {
                Data = _mapper.Map<UserViewModel>(user),
                Message = "Create successful"
            };
        }
        /// <summary>
        /// Đăng nhập với username và password
        /// </summary>
        /// <returns></returns>
        [HttpPost("login")]
        public AuthenticateResponse Login([FromBody] LoginRequest login)
        {
            var user = _authenticator.Authenticate(login.UserName, login.Password);
            var token = _authenticator.GenerateJsonWebToken(user, _appSettings.Jwt.Key, _appSettings.Jwt.Issuer);
            return new AuthenticateResponse
            {
                AccessToken = token,
                User = _mapper.Map<UserViewModel>(user),
            };
        }

    }
}
