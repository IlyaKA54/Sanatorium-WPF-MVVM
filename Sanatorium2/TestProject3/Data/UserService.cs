using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject3.Data
{
    public class UserService
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
}
