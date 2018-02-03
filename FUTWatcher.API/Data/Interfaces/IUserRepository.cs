using FUTWatcher.API.Models;

namespace FUTWatcher.API.Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User Authenticate(string username, string password);
        User Create(User user, string password);
    }
}