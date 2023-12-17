using Microsoft.EntityFrameworkCore;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sanatorium.Model.Repositories.Entities;

public class OrderRepository : IRepository<Order>
{
    private SanatoriumContext _context;
    public OrderRepository(SanatoriumContext context)
    {
        _context = context;
    }

    public void Create(Order item)
    {
        _context.Orders.Add(item);
    }

    public void Delete(Order item)
    {
        _context.Orders.Remove(item);
    }

    public IEnumerable<Order> GetCollection()
    {
        return new ObservableCollection<Order>(_context.Orders.ToList());
    }

    public Order GetItem(int id)
    {
        return _context.Orders.Find(id);
    }

    public void Update(Order item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }
}
