using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories;
using Sanatorium.Model.Repositories.Interface;
using Sanatorium.View;
using Sanatorium.ViewModel.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sanatorium.ViewModel;

public class CustomersViewModel : ViewModelBase
{
    private ObservableCollection<Customer> _customers;
    private Window _activeWindow;
    private string? _searchText;

    private IDbRepos _repos;
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

    public CustomersViewModel()
    {
        _selectedCustomers = new ObservableCollection<Customer>();

        ShowAdditionalWindowCommand = new ViewModelCommand(ExecuteShowAdditionalWindowCommand);
        RefreshCommand = new ViewModelCommand(ExecuteRefreshCommand);
        DeleteCustomerCommand = new ViewModelCommand(ExecuteDeleteCustomerCommand);
        ShowEditWindowCommand = new ViewModelCommand(ExecuteShowEditWindowCommand);

        LoadCustomers();
    }

    private void ExecuteRefreshCommand(object obj)
    {
        LoadCustomers(_searchText);
    }

    private void ExecuteShowAdditionalWindowCommand(object obg)
    {
        if (_activeWindow == null || _activeWindow.DataContext == null)
        {
            _activeWindow = new AdditionCustomerView();
            (_activeWindow.DataContext as AdditionCustomerViewModel).Close += CloseWindow;
            _activeWindow.Show();
        }
    }

    private void ExecuteShowEditWindowCommand(object obj)
    {
        if (obj is Customer customer)
        {
            if (_activeWindow == null || _activeWindow.DataContext == null)
            {
                var editViewModel = new EditingACustomerViewModel(customer);

                editViewModel.Close += CloseWindow;

                _activeWindow = new EditingACustomerView
                {
                    DataContext = editViewModel
                };

                _activeWindow.Show();
            }
        }
    }

    private void ExecuteDeleteCustomerCommand(object parameter)
    {
        if (parameter is Customer customerToRemove)
        {
            Customers.Remove(customerToRemove);
            _repos.Customers.Delete(customerToRemove);

            _repos.Save();

            LoadCustomers();
        }
    }
    private void CloseWindow()
    {
        if (_activeWindow != null)
        {
            _activeWindow.Close();
            _activeWindow = null;
            LoadCustomers();
        }
    }

    private void LoadCustomers(string? str = null)
    {
        _repos = new DbEFRepos();

        if (string.IsNullOrEmpty(str))
            Customers = new ObservableCollection<Customer>(_repos.Customers.GetCollection());
        else
            Customers = new ObservableCollection<Customer>(_repos.Customers.GetCollection().Where(c => c.SecondName.StartsWith(str) ||
            c.Surname.StartsWith(str) || c.FirstName.StartsWith(str) || c.Phone.StartsWith(str)).ToList());

    }
}
