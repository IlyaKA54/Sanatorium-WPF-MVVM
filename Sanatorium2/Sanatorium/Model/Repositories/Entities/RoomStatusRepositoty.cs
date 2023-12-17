using Microsoft.EntityFrameworkCore;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sanatorium.Model.Repositories.Entities;

public class RoomStatusRepositoty : IRepository<RoomStatus>
{
    private SanatoriumContext _context;
    public RoomStatusRepositoty(SanatoriumContext context)
    {
        _context = context;
    }

    public void Create(RoomStatus item)
    {
        _context.RoomStatuses.Add(item);
    }

    public void Delete(RoomStatus item)
    {
        _context.RoomStatuses.Remove(item);
    }

    public IEnumerable<RoomStatus> GetCollection()
    {
        return new ObservableCollection<RoomStatus>(_context.RoomStatuses.ToList());
    }

    public RoomStatus GetItem(int id)
    {
        return _context.RoomStatuses.Find(id);
    }

    public void Update(RoomStatus item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }
}
