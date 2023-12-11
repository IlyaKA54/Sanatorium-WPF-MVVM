using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanatorium.Model.Repositories;

public class LoginRepository : IRepository<User>
{
    public void Create(User item)
    {
        throw new NotImplementedException();
    }

    public void Delete(User item)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User> GetCollection()
    {
        throw new NotImplementedException();
    }

    public User GetItem(int id)
    {
        return null;
    }

    public void Update(User item)
    {
        throw new NotImplementedException();
    }
}
