using Microsoft.EntityFrameworkCore;
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

public class HomeViewModel : ViewModelBase
{
    private ObservableCollection<CustomerOrder> _orders;
    private IDbRepos _repos;

    public ObservableCollection<CustomerOrder> Orders
    {
        get
        {
            return _orders;
        }
        set
        {
            _orders = value;
            OnPropertyChanged();
        }
    }

    public ICommand ShowCustomerSelectionCommand { get; private set; }

    public HomeViewModel()
    {
        ShowCustomerSelectionCommand = new ViewModelCommand(ExecuteShowCustomerSelectionCommand);
        _repos = new DbEFRepos();
        LoadOrders();
    }

    private void ExecuteShowCustomerSelectionCommand(object obj)
    {
        var newWindow = new CustomerSelectionView();
        newWindow.Show();
    }

    private void LoadOrders()
    {
        var currentDate = DateTime.Now;

        var orders = _repos.CustomerOrders
            .GetCollection()
            .Where(co => co.IdOrder.DateOfDeparture > currentDate)
            .ToList();

        Orders = new ObservableCollection<CustomerOrder>(orders);
    }
}
