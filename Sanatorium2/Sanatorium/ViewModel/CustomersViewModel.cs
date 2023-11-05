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
        private ObservableCollection<Customer> customers;
        private AdditionCustomerView _checkView;
        private string? _searchText;
        public ObservableCollection<Customer> Customers
        {
            get
            {
                return customers;
            }
            set
            {
                customers = value;
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
        public ICommand RefreshCommand { get; private set; }
        public CustomersViewModel()
        {
            Customers = new ObservableCollection<Customer>();
            ShowAdditionalWindowCommand = new ViewModelCommand(ExecuteShowAdditionalWindowCommand);
            RefreshCommand = new ViewModelCommand(ExecuteRefreshCommand);
            LoadCustomers();
        }

        private void ExecuteRefreshCommand(object obj)
        {
            LoadCustomers(_searchText);
        }

        private void ExecuteShowAdditionalWindowCommand(object obg)
        {
            if (_checkView == null || _checkView.DataContext == null)
            {
                _checkView = new AdditionCustomerView();
                (_checkView.DataContext as AdditionCustomerViewModel).Close += Close;
                _checkView.Show();
            }
        }
        private void Close()
        {
            if(_checkView != null)
            {
                _checkView.Close();
                _checkView = null;
                Debug.Write(1);
            }
        }
        public void LoadCustomers(string? str = null)
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
