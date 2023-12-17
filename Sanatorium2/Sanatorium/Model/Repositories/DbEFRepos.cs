using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories.Entities;
using Sanatorium.Model.Repositories.Interface;

namespace Sanatorium.Model.Repositories;

public class DbEFRepos : IDbRepos
{
    private SanatoriumContext _context;

    private OrderRepository _orderRepository;
    private UserRepository _userRepository;
    private CustomerOrderRepository _customerOrderRepository;
    private CustomerRepository _customerRepository;
    private RoomRepository _roomRepository;
    private RoomStatusRepositoty _roomStatusRepository;
    private TypeOfRoomRepository _typeOfRoomRepository;
    private TreatmentProgramRepository _treatmentProgramRepository;

    public DbEFRepos()
    {
        _context = new SanatoriumContext();
    }

    public IRepository<Customer> Customers
    {
        get
        {
            if( _customerRepository == null )
                _customerRepository = new CustomerRepository( _context );
            return _customerRepository;
        }
    }

    public IRepository<Room> Rooms
    {
        get
        {
            if (_roomRepository == null)
                _roomRepository = new RoomRepository(_context);

            return new RoomRepository(_context);
        }
    }

    public IRepository<TypeOfRoom> Types
    {
        get
        {
            if(_typeOfRoomRepository == null )
                _typeOfRoomRepository = new TypeOfRoomRepository( _context );

            return _typeOfRoomRepository;
        }
    }

    public IRepository<User> Users 
    {
        get
        {
            if (_userRepository == null)
                _userRepository = new UserRepository(_context);

            return _userRepository;
        }
    }

    public IRepository<Order> Orders
    {
        get
        {
            if (_orderRepository == null)
                _orderRepository = new OrderRepository(_context);

            return _orderRepository;
        }
    }

    public IRepository<TreatmentProgram> TreatmentPrograms
    {
        get
        {
            if(_treatmentProgramRepository == null)
                _treatmentProgramRepository = new TreatmentProgramRepository( _context );

            return _treatmentProgramRepository;
        }
    }

    public IRepository<CustomerOrder> CustomerOrders
    {
        get
        {
            if(_customerOrderRepository == null)
                _customerOrderRepository = new CustomerOrderRepository(_context);

            return _customerOrderRepository;
        }
    }

    public IRepository<RoomStatus> Statuses
    {
        get
        {
            if(_roomStatusRepository == null)
                _roomStatusRepository = new RoomStatusRepositoty(_context);

            return _roomStatusRepository;
        }
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
