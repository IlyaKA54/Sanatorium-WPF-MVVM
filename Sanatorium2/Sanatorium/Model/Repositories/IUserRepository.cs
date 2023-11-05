using System.Collections.Generic;
using System.Net;

namespace Sanatorium.Model.Repositories;

public interface IUserRepository
{
    bool AuthenticateUser(NetworkCredential credential);

}
