using Sanatorium.Model.Data;
using System.Linq;
using System.Net;

namespace Sanatorium.Model.Repositories;

public class UserRepository : IUserRepository
{

    public bool AuthenticateUser(NetworkCredential credential)
    {
        using (var context = new SanatoriumContext())
        {
            var user = context.Users
                .FirstOrDefault(u => u.Login == credential.UserName && u.Password == credential.Password);

            return user != null;
        }
    }
}
