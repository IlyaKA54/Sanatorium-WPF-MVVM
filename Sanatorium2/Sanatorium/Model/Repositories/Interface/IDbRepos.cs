using Sanatorium.Model.Entities;

namespace Sanatorium.Model.Repositories.Interface;

public interface IDbRepos
{
    IRepository<Customer> Customers { get; }
    IRepository<Room> Rooms { get; }
    IRepository<TypeOfRoom> Types { get; }
    IRepository<User> Users { get; }
    IRepository<Order> Orders { get; }
    IRepository<TreatmentProgram> Programs { get; }
    IRepository<CustomerOrder> CustomerOrders { get; }

    int Save();
}
