using System.Collections.Generic;

namespace Sanatorium.Model.Repositories.Interface;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetCollection();

    T GetItem(int id);

    void Create(T item);

    void Update(T item);

    void Delete(T item);
}
