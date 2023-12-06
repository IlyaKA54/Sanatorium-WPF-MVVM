using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.View;
using Sanatorium.ViewModel.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace Sanatorium.ViewModel
{
    public class CustomersViewModel : ViewModelBase
    {
        private ObservableCollection<Customer> _customers;
        private AdditionCustomerView _activeAdditionalCustomerView;
        private EditingACustomerView _activeEditingACustomerView;
        private string? _searchText;

        private ObservableCollection<Customer> _selectedCustomers;

        public ObservableCollection<Customer> Customers
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

        public string? SearchText
        {
            get
            {
                return _searchText;
            }

            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowAdditionalWindowCommand { get; private set; }

        public ICommand ShowEditWindowCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand DeleteCustomerCommand { get; private set; }

        public ICommand SelectACustomerCommand { get; private set; }
        public ICommand UnselectACustomerCommand { get; private set; }
        public CustomersViewModel()
        {
            Customers = new ObservableCollection<Customer>();
            _selectedCustomers = new ObservableCollection<Customer>();

            ShowAdditionalWindowCommand = new ViewModelCommand(ExecuteShowAdditionalWindowCommand);
            RefreshCommand = new ViewModelCommand(ExecuteRefreshCommand);
            DeleteCustomerCommand = new ViewModelCommand(ExecuteDeleteCustomerCommand);
            ShowEditWindowCommand = new ViewModelCommand(ExecuteShowEditWindowCommand);
            SelectACustomerCommand = new ViewModelCommand(ExecuteAddSelectedCustomerCommand);
            UnselectACustomerCommand = new ViewModelCommand(ExecuteRemoveSelectedCustomerCommand);

            LoadCustomers();
        }

        private void ExecuteRemoveSelectedCustomerCommand(object obj)
        {
            if (obj is Customer customer)
                if (!_selectedCustomers.Contains(customer))
                    _selectedCustomers.Add(customer);
                else
                    _selectedCustomers.Add(customer);

        }

        private void ExecuteAddSelectedCustomerCommand(object obj)
        {
            if(obj is Customer customer)
                if (!_selectedCustomers.Contains(customer))
                    _selectedCustomers.Add(customer);
                else
                    _selectedCustomers.Add(customer);

        }

        private void ExecuteRefreshCommand(object obj)
        {
            LoadCustomers(_searchText);
        }

        private void ExecuteShowAdditionalWindowCommand(object obg)
        {
            if (_activeAdditionalCustomerView == null || _activeAdditionalCustomerView.DataContext == null)
            {
                _activeAdditionalCustomerView = new AdditionCustomerView();
                (_activeAdditionalCustomerView.DataContext as AdditionCustomerViewModel).Close += CloseAdditionalCustomerWindow;
                _activeAdditionalCustomerView.Show();
            }
        }

        private void ExecuteShowEditWindowCommand(object obj)
        {
            if(obj is Customer customer)
            {
                if (_activeEditingACustomerView == null || _activeEditingACustomerView.DataContext == null)
                {
                    var editViewModel = new EditingACustomerViewModel(customer);

                    editViewModel.Close += CloseEditingACustomerView;

                    _activeEditingACustomerView = new EditingACustomerView
                    {
                        DataContext = editViewModel
                    };

                    _activeEditingACustomerView.Show();
                }
            }
        }

        private void ExecuteDeleteCustomerCommand(object parameter)
        {
            if (parameter is Customer customerToRemove)
            {
                Customers.Remove(customerToRemove);

                using (var context = new SanatoriumContext())
                {
                    context.Customers.Remove(customerToRemove);
                    context.SaveChanges();
                }
            }
        }
        private void CloseAdditionalCustomerWindow()
        {
            if(_activeAdditionalCustomerView != null)
            {
                _activeAdditionalCustomerView.Close();
                _activeAdditionalCustomerView = null;
            }
        }

        private void CloseEditingACustomerView()
        {
            if(_activeEditingACustomerView != null)
            {
                _activeEditingACustomerView.Close();
                _activeEditingACustomerView = null;
            }
        }
        private void LoadCustomers(string? str = null)
        {
            using (var context = new SanatoriumContext())
            {
                if (string.IsNullOrEmpty(str))
                    Customers = new ObservableCollection<Customer>(context.Customers.ToList());
                else
                    Customers = new ObservableCollection<Customer>(context.Customers.Where(c => c.SecondName.StartsWith(str) || 
                    c.Surname.StartsWith(str) || c.FirstName.StartsWith(str) || c.Phone.StartsWith(str)).ToList());
            }
        }
    }
}
