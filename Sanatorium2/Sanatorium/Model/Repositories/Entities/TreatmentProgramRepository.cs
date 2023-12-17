using Microsoft.EntityFrameworkCore;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sanatorium.Model.Repositories.Entities;

public class TreatmentProgramRepository : IRepository<TreatmentProgram>
{
    private SanatoriumContext _context;
    public TreatmentProgramRepository(SanatoriumContext context)
    {
        _context = context;
    }

    public void Create(TreatmentProgram item)
    {
        _context.TreatmentPrograms.Add(item);
    }

    public void Delete(TreatmentProgram item)
    {
        _context.TreatmentPrograms.Remove(item);
    }

    public IEnumerable<TreatmentProgram> GetCollection()
    {
        return new ObservableCollection<TreatmentProgram>(_context.TreatmentPrograms.ToList());
    }

    public TreatmentProgram GetItem(int id)
    {
        return _context.TreatmentPrograms.Find(id);
    }

    public void Update(TreatmentProgram item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }
}
