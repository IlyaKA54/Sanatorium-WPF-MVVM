using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject3.Data;

public interface IDbRepos
{

    IRepository<User> Users { get; }
    IRepository<Customer> Customers { get; }


    void Save();
}
