namespace Sanatorium.Model.Repositories.Interface;

public interface IUserService
{
    bool AuthenticateUser(string userName, string password);

}
