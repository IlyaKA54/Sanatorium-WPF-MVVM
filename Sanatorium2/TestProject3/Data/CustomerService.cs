using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject3.Data
{
    public class CustomerService
    {
        private IDbRepos _repos;

        public CustomerService(IDbRepos repos)
        {
            _repos = repos;
        }

        public Customer GetCustomer(int id)
        {
            return _repos.Customers.GetItem(id);
        }

        public IEnumerable<Customer> GetFilterList(string filter)
        {
            return _repos.Customers.GetCollection().Where(a => a.FirstName == filter);
        }

    }
}
