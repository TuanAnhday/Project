using Microsoft.AspNetCore.Identity;
using Project.Domain.Aggregates.User;
using Project.Domain.Contracts.Responsitoties;
using Project.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        //private readonly ProjectDbContext _dbContext;
        private readonly UserManager<UseDataModel> _userManager;

        public UserRepository( UserManager<UseDataModel> userManager)
        {
            //_dbContext = dbContext;
            _userManager = userManager;
        }
        public void AddUser(User user, string password)
        {
            var userDataModel =  new UseDataModel(user);
            var creatingResult = _userManager.CreateAsync(userDataModel, password).Result;
            if (!creatingResult.Succeeded) 
                throw new ArgumentException("Error creating user");
        }

        public User GetUserByUserName(string username)
        {
            if(username == null) return null;
            var user = _userManager.FindByNameAsync(username).Result;
            return user?.ToEntity();
        }
    }
}
