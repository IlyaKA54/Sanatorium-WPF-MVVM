
using Sanatorium.Model.Entities;
using System.Net;

namespace Sanatorium.Model.Repositories;

public interface IUserRepository
{
    bool AuthenticateUser(NetworkCredential credential);

}
