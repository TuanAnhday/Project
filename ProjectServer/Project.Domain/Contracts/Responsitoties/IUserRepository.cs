using Project.Domain.Aggregates.User;

namespace Project.Domain.Contracts.Responsitoties;

public interface IUserRepository
{
    User GetUserByUserName(string username);
    void AddUser(User user, string password);
}
