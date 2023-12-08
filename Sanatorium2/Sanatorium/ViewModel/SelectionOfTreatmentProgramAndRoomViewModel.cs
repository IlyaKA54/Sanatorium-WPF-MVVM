﻿using Microsoft.EntityFrameworkCore;
using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.ViewModel.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Sanatorium.ViewModel
{
    public class SelectionOfTreatmentProgramAndRoomViewModel : ViewModelBase
    {
        private ObservableCollection<CustomerOrder> _customers;
        private ObservableCollection<Room> _rooms;
        private ObservableCollection<TreatmentProgram> _programs;
        private DateTime _arrivalDate = DateTime.Now;
        private DateTime _dateOfDeparture = DateTime.Now;

        private decimal _price;

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
                return _customers;
            }
            set
            {
                _customers = value;
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

        public Action Close;

        public SelectionOfTreatmentProgramAndRoomViewModel(ObservableCollection<Customer> customers)
        {
            _customers = new ObservableCollection<CustomerOrder>();

            CreateOrderCommand = new ViewModelCommand(ExecuteCreateOrderCommand, CanExecuteCreateOrderCommand);

            LoadCustomers(customers);

            LoadRooms();
            LoadTreatmentPrograms();
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

            foreach (var customer in _customers)
            {
                if (customer.TreatmentProgram == null || customer.Room == null)
                    return false;
            }

            return true;
        }

        private void ExecuteCreateOrderCommand(object obj)
        {
            CalculatePrice();

            using (var context = new SanatoriumContext())
            {
                context.Orders.Add(GetNewOrder());
                context.SaveChanges();
            }

            SaveCustomerOrders();
        }

        private void SaveCustomerOrders()
        {
            using (var context = new SanatoriumContext())
            {
                var last = context.Orders.OrderByDescending(order => order.Id).First();

                foreach (var customer in _customers)
                {
                    customer.IdOrder = last;
                    context.CustomerOrders.Add(customer);
                    context.SaveChanges();
                }

            }
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
            foreach (var customer in _customers)
            {
                decimal discount;

                if (customer.Customer.NumberOfVisits == 10)
                    discount = (decimal)0.5;
                else
                    discount = (decimal)(1 - (0.05 * customer.Customer.NumberOfVisits));

                _price += (customer.TreatmentProgram.Price + customer.Room.Price) * discount;

                AddAVisit(customer.Customer);
            }
        }

        private void AddAVisit(Customer customer)
        {
            customer.NumberOfVisits++;
        }

        private void LoadCustomers(ObservableCollection<Customer> customers)
        {
            foreach (var customer in customers)
            {
                _customers.Add(new CustomerOrder { Customer = customer });
            }
        }

        public void LoadRooms()
        {
            using (var context = new SanatoriumContext())
            {
                Rooms = new ObservableCollection<Room>(context.Rooms.Include(a => a.Type).Include(a => a.Status).ToList());
            }
        }

        private void LoadTreatmentPrograms()
        {
            using (var context = new SanatoriumContext())
            {
                Programs = new ObservableCollection<TreatmentProgram>(context.TreatmentPrograms.ToList());
            }
        }
    }
}
