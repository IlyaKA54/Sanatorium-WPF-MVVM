using Microsoft.EntityFrameworkCore;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories;
using Sanatorium.Model.Repositories.Interface;
using Sanatorium.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;

namespace Sanatorium.ViewModel
{
    public class SelectionOfTreatmentProgramAndRoomViewModel : ViewModelBase
    {
        private ObservableCollection<CustomerOrder> _customerOrders;
        private ObservableCollection<Room> _rooms;
        private ObservableCollection<TreatmentProgram> _programs;
        private DateTime _arrivalDate = DateTime.Now;
        private DateTime _dateOfDeparture = DateTime.Now;

        private decimal _price;
        private IDbRepos _repos;

        public DateTime ArrivalDate
        {
            get
            {
                return _arrivalDate;
            }
            set
            {
                _arrivalDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateOfDeparture
        {
            get
            {
                return _dateOfDeparture;
            }
            set
            {
                _dateOfDeparture = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CustomerOrder> Customers
        {
            get
            {
                return _customerOrders;
            }
            set
            {
                _customerOrders = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Room> Rooms
        {
            get
            {
                return _rooms;
            }
            set
            {
                _rooms = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TreatmentProgram> Programs
        {
            get
            {
                return _programs;
            }
            set
            {
                _programs = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreateOrderCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }

        public Action Close;

        public SelectionOfTreatmentProgramAndRoomViewModel(ObservableCollection<Customer> customers)
        {
            _repos = new DbEFRepos();
            _customerOrders = new ObservableCollection<CustomerOrder>();

            CreateOrderCommand = new ViewModelCommand(ExecuteCreateOrderCommand, CanExecuteCreateOrderCommand);
            CloseCommand = new ViewModelCommand(ExecuteCloseCommand);

            LoadCustomers(customers);

            LoadRooms();
            LoadTreatmentPrograms();
        }

        private void ExecuteCloseCommand(object obj)
        {
            Close?.Invoke();
        }

        private bool CanExecuteCreateOrderCommand(object obj)
        {
            if (_arrivalDate < DateTime.Now.AddDays(-1) || _arrivalDate > _dateOfDeparture || !ValidateCustomerOrderList())
                return false;
            else
                return true;
        }

        private bool ValidateCustomerOrderList()
        {

            foreach (var customer in _customerOrders)
                if (customer.TreatmentProgram == null || customer.Room == null)
                    return false;

            return true;
        }

        private void ExecuteCreateOrderCommand(object obj)
        {
            CalculatePrice();

            _repos.Orders.Create(GetNewOrder());

            SaveCustomerOrders();
            AddAVisit();
            Close?.Invoke();
        }

        private void SaveCustomerOrders()
        {
            var lastOrder = _repos.Orders.GetCollection().LastOrDefault();

            foreach (var customer in _customerOrders)
            {
                var newCustomerOrder = GetNewCustomerOrder(customer, lastOrder);

                _repos.CustomerOrders.Create(newCustomerOrder);
            }

            _repos.Save();
        }

        private CustomerOrder GetNewCustomerOrder(CustomerOrder customer, Order lastOrder )
        {
            var newCustomerOrder = new CustomerOrder
            {
                Customer = _repos.Customers.GetItem(customer.Customer.Id),
                Room = _repos.Rooms.GetItem(customer.Room.Id),
                TreatmentProgram = _repos.TreatmentPrograms.GetItem(customer.TreatmentProgram.Id),
                IdOrder = lastOrder
            };

            return newCustomerOrder;
        }

        private Order GetNewOrder()
        {
            return new Order
            {
                ArrivalDate = _arrivalDate,
                DateOfDeparture = _dateOfDeparture,
                Price = _price,
            };
        }

        private void CalculatePrice()
        {
            foreach (var customer in _customerOrders)
            {
                decimal discount;

                if (customer.Customer.NumberOfVisits == 10)
                    discount = (decimal)0.5;
                else
                    discount = (decimal)(1 - (0.05 * customer.Customer.NumberOfVisits));

                _price += (customer.TreatmentProgram.Price + customer.Room.Price) * discount;

            }
        }

        private void AddAVisit()
        {
            var customerIds = _customerOrders.Select(customerOrder => customerOrder.Customer.Id).ToList();

            foreach (var customerId in customerIds)
            {
                var customer = _repos.Customers.GetItem(customerId);
                customer.NumberOfVisits++;
            }

            _repos.Save();
        }
        private void LoadCustomers(ObservableCollection<Customer> customers)
        {
            foreach (var customer in customers)
            {
                _customerOrders.Add(new CustomerOrder { Customer = customer });
            }
        }

        public void LoadRooms()
        {
            var currentDate = DateTime.Now;

            var availableRooms = GetAvailableRooms(currentDate);

            Rooms = new ObservableCollection<Room>(availableRooms);
        }

        private List<Order> GetActiveOrders(DateTime currentDate)
        {
            return _repos.Orders.GetCollection()
                .Where(order => currentDate >= order.ArrivalDate && currentDate <= order.DateOfDeparture)
                .ToList();
        }

        private List<CustomerOrder> GetCustomerOrders(List<Order> activeOrders)
        {
            return activeOrders
                .SelectMany(order => _repos.CustomerOrders.GetCollection().Where(co => co.IdOrder.Id == order.Id))
                .ToList();
        }

        private List<Room> GetAvailableRooms(DateTime currentDate)
        {
            var activeOrders = GetActiveOrders(currentDate);
            var customerOrders = GetCustomerOrders(activeOrders);

            return _repos.Rooms.GetCollection()
                .Where(room => room.NumberOfPlaces - customerOrders.Count(co => co.Room.Id == room.Id) >= _customerOrders.Count)
                .ToList();
        }

        private void LoadTreatmentPrograms()
        {
            Programs = new ObservableCollection<TreatmentProgram>(_repos.TreatmentPrograms.GetCollection());
        }
    }
}
