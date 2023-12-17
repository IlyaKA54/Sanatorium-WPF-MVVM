using Microsoft.EntityFrameworkCore;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sanatorium.Model.Repositories.Entities;

public class CustomerOrderRepository : IRepository<CustomerOrder>
{
    private SanatoriumContext _context;
    public CustomerOrderRepository(SanatoriumContext context)
    {
        _context = context;
    }

    public void Create(CustomerOrder item)
    {
        _context.CustomerOrders.Add(item);
    }

    public void Delete(CustomerOrder item)
    {
        _context.CustomerOrders.Remove(item);
    }

    public IEnumerable<CustomerOrder> GetCollection()
    {
        return new ObservableCollection<CustomerOrder>(_context.CustomerOrders
            .Include(a => a.Customer)
            .Include(a => a.IdOrder)
            .Include(a => a.TreatmentProgram)
            .ToList());
    }

    public CustomerOrder GetItem(int id)
    {
        return _context.CustomerOrders
            .Include(a => a.Room)
            .Include(a => a.Customer)
            .Include(a => a.IdOrder)
            .Include(a => a.TreatmentProgram)
            .FirstOrDefault(a => a.Id == id);
    }   

    public void Update(CustomerOrder item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }
}
