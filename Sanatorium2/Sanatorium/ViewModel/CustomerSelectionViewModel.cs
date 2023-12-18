using Sanatorium.Model.Data;
using Sanatorium.Model.Entities;
using Sanatorium.Model.Repositories;
using Sanatorium.Model.Repositories.Interface;
using Sanatorium.View;
using Sanatorium.ViewModel.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Sanatorium.ViewModel;

public class CustomerSelectionViewModel : ViewModelBase
{
    private ObservableCollection<Customer> _allCustomers;
    private ObservableCollection<Customer> _selectedCustomers;

    private ObservableCollection<Customer> _startList;

    private IDbRepos _repos;
    private string _searchText;

    public string SearchText
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

    public ObservableCollection<Customer> AllCustomers
    {
        get
        {
            return _allCustomers;
        }
        set
        {
            _allCustomers = value;
            OnPropertyChanged();
        }
    }


    public ObservableCollection<Customer> SelectedCustomers
    {
        get
        {
            return _selectedCustomers;
        }
        set
        {
            _selectedCustomers = value;
            OnPropertyChanged();
        }
    }

    public ICommand MoveACustomerCommand { get; private set; }
    public ICommand RefreshCommand { get; private set; }
    public ICommand CheckInCustomersCommand { get; private set; }
    public ICommand CloseCommand { get; private set; }

    public Action Close;
    public CustomerSelectionViewModel()
    {
        _startList = new ObservableCollection<Customer>();
        _selectedCustomers = new ObservableCollection<Customer>();
        _repos = new DbEFRepos();

        MoveACustomerCommand = new ViewModelCommand(ExecuteMoveACustomerCommand);
        RefreshCommand = new ViewModelCommand(ExecuteRefreshCommand);
        CheckInCustomersCommand = new ViewModelCommand(ExecuteCheckInCustomerCommand, CanExecuteCheckInCustomerCommand);
        CloseCommand = new ViewModelCommand(ExecuteCloseCommand);

        LoadCustomers();
        _allCustomers = new ObservableCollection<Customer>(_startList);
    }

    private bool CanExecuteCheckInCustomerCommand(object obj)
    {
        if (_selectedCustomers == null || _selectedCustomers.Count == 0)
            return false;

        return true;
    }

    private void ExecuteCloseCommand(object obj)
    {
        Close?.Invoke();
    }

    private void ExecuteCheckInCustomerCommand(object obj)
    {
        var newWindow = new SelectionOfTreatmentProgramAndRoomView();
        newWindow.SetDataContext(new SelectionOfTreatmentProgramAndRoomViewModel(_selectedCustomers));
        newWindow.Show();
        Close?.Invoke();

    }

    private void ExecuteRefreshCommand(object obj)
    {
        if (string.IsNullOrEmpty(_searchText))
            AllCustomers = new ObservableCollection<Customer>(_startList);
        else
            AllCustomers = new ObservableCollection<Customer>(_startList.Where(c => c.SecondName.StartsWith(_searchText) ||
                c.Surname.StartsWith(_searchText) || c.FirstName.StartsWith(_searchText) || c.Phone.StartsWith(_searchText)).ToList());
    }

    private void ExecuteMoveACustomerCommand(object obj)
    {
        if (obj is Customer customer)
        {
            if (AllCustomers.Contains(customer))
            {
                AllCustomers.Remove(customer);
                SelectedCustomers.Add(customer);
            }
            else
            {
                SelectedCustomers.Remove(customer);
                AllCustomers.Add(customer);
            }
        }
    }

    private void LoadCustomers(string? str = null)
    {
        _startList = new ObservableCollection<Customer>(_repos.Customers.GetCollection());
    }
}
