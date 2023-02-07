using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Domain.Aggregates.User;
using Project.Domain.Contracts.Responsitoties;
using Project.Infrastructure.Utils.Helpers;
using Project.Web.Requests.CreateRequests.User;
using Project.Web.Responses.CreateResponses;
using Project.Web.ViewModels;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userReposity;
        private readonly Authenticator _authenticator;
        public UsersController (
            IMapper mapper,
            IUserRepository userRepository,
            Authenticator authenticator
            )
        { 
            _mapper = mapper;
            _userReposity = userRepository;
            _authenticator = authenticator;
        }

        public static UsersController CreateInstance(IMapper mapper, IUserRepository userRepository, Authenticator authenticator) 
        { 
            return new UsersController (mapper, userRepository, authenticator); 
        }

        /// <summary>
        /// Tạo tài khoản người dùng
        /// </summary>
        /// <returns></returns>
        [HttpPost("register")]
        public CreateResponses<UserViewModel> Register([FromBody] RegisterRequest request)
        {
            var user = _userReposity.GetUserByUserName(request.UserName);
            if (user == null) throw new ArgumentException($"User name {request.UserName} already taken");
            _authenticator.ValidatePassword(request.Password);
            user = new User(Guid.NewGuid())
            {
                UserName = request.UserName,
                FullName = request.FullName,
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


    }
}
