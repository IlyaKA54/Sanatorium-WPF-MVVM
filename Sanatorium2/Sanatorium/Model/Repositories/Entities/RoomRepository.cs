using Microsoft.EntityFrameworkCore;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sanatorium.Model.Repositories.Entities;

public class RoomRepository : IRepository<Room>
{
    private SanatoriumContext _context;
    public RoomRepository(SanatoriumContext context)
    {
        _context = context;
    }

    public void Create(Room item)
    {
        _context.Rooms.Add(item);
    }

    public void Delete(Room item)
    {
        _context.Rooms.Remove(item);
    }

    public IEnumerable<Room> GetCollection()
    {
        return new ObservableCollection<Room>(_context.Rooms
            .Include(a => a.Status)
            .Include(a => a.Type)
            .ToList());
    }

    public Room GetItem(int id)
    {
        return _context.Rooms
            .Include(a => a.Status)
            .Include(a => a.Type)
            .FirstOrDefault(a => a.Id == id);
    }

    public void Update(Room item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }
}
