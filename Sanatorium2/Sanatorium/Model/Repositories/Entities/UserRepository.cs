using Microsoft.EntityFrameworkCore;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sanatorium.Model.Repositories.Entities;

public class UserRepository : IRepository<User>
{
    private SanatoriumContext _context;

    public UserRepository(SanatoriumContext context)
    {
        _context = context;
    }

    public void Create(User item)
    {
        _context.Users.Add(item);
    }

    public void Delete(User item)
    {
        _context.Users.Remove(item);
    }

    public IEnumerable<User> GetCollection()
    {
        return new ObservableCollection<User>(_context.Users.ToList());
    }

    public User GetItem(int id)
    {
        return _context.Users.Find(id);
    }

    public void Update(User item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }
}
