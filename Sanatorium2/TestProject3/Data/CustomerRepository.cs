using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace TestProject3.Data
{
    public class CustomerRepository : IRepository<Customer>
    {
        private SanatoriumContext _context;
        public CustomerRepository(SanatoriumContext context)
        {
            _context = context;
        }

        public void Create(Customer item)
        {
            _context.Customers.Add(item);
        }

        public void Delete(Customer item)
        {
            _context.Customers.Remove(item);
        }

        public IEnumerable<Customer> GetCollection()
        {
            return new ObservableCollection<Customer>(_context.Customers.ToList());
        }

        public Customer GetItem(int id)
        {
            return _context.Customers.Find(id);
        }

        public void Update(Customer item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
