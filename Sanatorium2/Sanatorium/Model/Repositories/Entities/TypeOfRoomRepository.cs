using Microsoft.EntityFrameworkCore;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sanatorium.Model.Repositories.Entities;

public class TypeOfRoomRepository : IRepository<TypeOfRoom>
{
    private SanatoriumContext _context;
    public TypeOfRoomRepository(SanatoriumContext context)
    {
        _context = context;
    }

    public void Create(TypeOfRoom item)
    {
        _context.TypeOfRooms.Add(item);
    }

    public void Delete(TypeOfRoom item)
    {
        _context.TypeOfRooms.Remove(item);
    }

    public IEnumerable<TypeOfRoom> GetCollection()
    {
        return new ObservableCollection<TypeOfRoom>(_context.TypeOfRooms.ToList());
    }

    public TypeOfRoom GetItem(int id)
    {
        return _context.TypeOfRooms.Find(id);
    }

    public void Update(TypeOfRoom item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }
}
