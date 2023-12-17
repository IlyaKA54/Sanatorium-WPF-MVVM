using Sanatorium.Model.Repositories.Interface;
using System.Linq;

namespace Sanatorium.Model.Repositories;

public class UserService : IUserService
{

    private IDbRepos _repos;

    public UserService(IDbRepos repos)
    {

        _repos = repos;

    }

    public bool AuthenticateUser(string userName, string password)
    {
        var users = _repos.Users.GetCollection();

        var user = users.FirstOrDefault(w => w.Login == userName && w.Password == password);

        return user != null;

    }


}
